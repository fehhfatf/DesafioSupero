using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DS.Domain.Entities;
using DS.UI.PrestadorDeServico.Models;
using DS.Domain.Interfaces.Repositories;
using DS.Infra.Data.Repository;

namespace DS.UI.PrestadorDeServico.Controllers
{
    public class ServicosController : Controller
    {
        private readonly IRepository<Cliente> repCliente;
        private readonly IRepository<Fornecedor> repFornecedor;
        private readonly IRepository<Servico> repServico;

        public ServicosController()
        {
            repCliente = new Repository<Cliente>();
            repFornecedor = new Repository<Fornecedor>();
            repServico = new Repository<Servico>();
        }
        // GET: Servicos
        [Authorize]
        public ActionResult Index()
        {
            return View(repServico.ObterTodos());
        }

        
        public ActionResult Os3ClientesQueMaisGastaramPorMes()
        {
            List<EstatisticaClientePorMesViewModel> clientesReturn = new List<EstatisticaClientePorMesViewModel>();
            List<EstatisticaClientePorMesViewModel> clientes;
            EstatisticaClientePorMesViewModel cliente;
            string nomeDoCliente = "";
            decimal valor = 0;
            for (int i = 1; i <= 12; i++)
            {
                clientes = new List<EstatisticaClientePorMesViewModel>();
                var servicos = repServico.Buscar(c => c.DataDeAtendimento.Year == DateTime.Now.Year && c.DataDeAtendimento.Month == i).OrderBy(c => c.Cliente.Nome);
                if (servicos.Count() != 0)
                {
                    nomeDoCliente = servicos.FirstOrDefault().Cliente.Nome;
                    foreach (var item in servicos)
                    {
                        if (item.Cliente.Nome != nomeDoCliente)
                        {
                            cliente = new EstatisticaClientePorMesViewModel()
                            {
                                Mes = MesLiteral(i),
                                NomeDoCliente = nomeDoCliente,
                                Valor = valor
                            };
                            clientes.Add(cliente);
                            nomeDoCliente = "";
                            valor = 0;
                        }
                        nomeDoCliente = item.Cliente.Nome;
                        valor += item.ValorDoAtendimento;
                    }
                    cliente = new EstatisticaClientePorMesViewModel()
                    {
                        Mes = MesLiteral(i),
                        NomeDoCliente = nomeDoCliente,
                        Valor = valor
                    };
                    clientes.Add(cliente);
                    foreach (var item in clientes.OrderByDescending(c => c.Valor).Take(3))
                    {
                        clientesReturn.Add(item);
                    }
                }
            }

            return View(clientesReturn);
        }

        private string MesLiteral(int mes)
        {
            switch (mes)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";
                default:
                    return "Erro: Existe mês atendimento invalido!";
            }
        }

        
        public ActionResult MediaDoValorCobrado()
        {
            var servicos = repServico.Buscar(c => c.DataDeAtendimento.Year == DateTime.Now.Year);

            var MediaReturn = from s in servicos
                              group s by new { s.FornecedorId, s.TipoDeServico } into sg
                              select new EstatisticaMediaValorFornecedor
                              {
                                  NomeDoFornecedor = repFornecedor.Buscar(f => f.FornecedorId == sg.Key.FornecedorId).FirstOrDefault().Nome,
                                  TipoDeServico = sg.Key.TipoDeServico,
                                  ValorMedio = sg.Average(c => c.ValorDoAtendimento)
                              };

            return View(MediaReturn.OrderByDescending(s => s.ValorMedio));
        }

        
        public ActionResult FornecedorSemPrestarServicoNoMes()
        {
            List<EstatisticaFornecedorSemPrestarServicoPorMesViewModel> FornecedoresReturn = new List<EstatisticaFornecedorSemPrestarServicoPorMesViewModel>();
            var fornecedores = repFornecedor.ObterTodos();
            for (int i = 1; i <= DateTime.Now.Month; i++)
            {
                foreach (var item in fornecedores.OrderBy(f => f.Nome))
                {
                    var servicos = repServico.Buscar(c => c.DataDeAtendimento.Year == DateTime.Now.Year && c.DataDeAtendimento.Month == i && c.FornecedorId == item.FornecedorId);
                    if (servicos.Count() == 0)
                    {
                        FornecedoresReturn.Add(new EstatisticaFornecedorSemPrestarServicoPorMesViewModel()
                        {
                            Mes = MesLiteral(i),
                            NomeDoFornecedor = item.Nome
                        });
                    }
                }
            }

            return View(FornecedoresReturn);
        }
        [Authorize]
        public ActionResult ServicoPorPeriodo(string de, string ate, string cliente, string estado, string cidade, string bairro, string tipoDeServico)
        {
            IEnumerable<Servico> servicos = repServico.ObterTodos();
            if (Request.IsAjaxRequest())
            {
                if (de != "")
                {                                        
                    servicos = servicos.Where(s => s.DataDeAtendimento >= Convert.ToDateTime(de));
                }
                    
                if (ate != "")
                {                    
                    servicos = servicos.Where(s => s.DataDeAtendimento <= Convert.ToDateTime(ate));   
                }

                if (cliente != "")
                {
                    servicos = servicos.Where(s => s.Cliente.Nome.Contains(cliente));
                }

                if (estado != "")
                {
                    servicos = servicos.Where(s => s.Cliente.Estado.Contains(estado));
                }

                if (cidade != "")
                {
                    servicos = servicos.Where(s => s.Cliente.Cidade.Contains(cidade));
                }

                if (bairro != "")
                {
                    servicos = servicos.Where(s => s.Cliente.Bairro.Contains(bairro));
                }

                if (tipoDeServico != "")
                {
                    servicos = servicos.Where(s => s.TipoDeServico.Contains(tipoDeServico));
                }
                                 
                return PartialView("_Servicos", servicos);
            }       
            return View(servicos);
        }

        [Authorize]
        // GET: Servicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = repServico.ObterPorId(id.Value);
            if (servico == null)
            {
                return HttpNotFound();
            }
            return View(servico);
        }

        [Authorize]
        // GET: Servicos/Create
        public ActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(repCliente.ObterTodos(), "ClienteId", "Nome");
            ViewBag.FornecedorId = new SelectList(repFornecedor.ObterTodos(), "FornecedorId", "Nome");
            return View();
        }

        [Authorize]
        // POST: Servicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServicoId,ClienteId,FornecedorId,Descricao,DataDeAtendimento,ValorDoAtendimento,TipoDeServico")] Servico servico)
        {
            if (ModelState.IsValid)
            {
                repServico.Adicionar(servico);
                repServico.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteId = new SelectList(repCliente.ObterTodos(), "ClienteId", "Nome", servico.ClienteId);
            ViewBag.FornecedorId = new SelectList(repFornecedor.ObterTodos(), "FornecedorId", "Nome", servico.FornecedorId);
            return View(servico);
        }

        [Authorize]
        // GET: Servicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = repServico.ObterPorId(id.Value);
            if (servico == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteId = new SelectList(repCliente.ObterTodos(), "ClienteId", "Nome", servico.ClienteId);
            ViewBag.FornecedorId = new SelectList(repFornecedor.ObterTodos(), "FornecedorId", "Nome", servico.FornecedorId);
            return View(servico);
        }

        [Authorize]
        // POST: Servicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServicoId,ClienteId,FornecedorId,Descricao,DataDeAtendimento,ValorDoAtendimento,TipoDeServico")] Servico servico)
        {
            if (ModelState.IsValid)
            {
                repServico.Atualizar(servico);
                repServico.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteId = new SelectList(repCliente.ObterTodos(), "ClienteId", "Nome", servico.ClienteId);
            ViewBag.FornecedorId = new SelectList(repFornecedor.ObterTodos(), "FornecedorId", "Nome", servico.FornecedorId);
            return View(servico);
        }

        [Authorize]
        // GET: Servicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = repServico.ObterPorId(id.Value);
            if (servico == null)
            {
                return HttpNotFound();
            }
            return View(servico);
        }
        [Authorize]
        // POST: Servicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repServico.Remover(id);
            repServico.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repCliente.Dispose();
                repFornecedor.Dispose();
                repServico.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
