using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System.Collections.Generic;
using System.Linq;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController(FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Machine> model = _db.Machines.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Machine Machine)
    {
      _db.Machines.Add(Machine);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisEngineer = _db.Machines
          .Include(Machine => Machine.JoinEntities)
          .ThenInclude(join => join.Machine)
          .FirstOrDefault(Machine => Machine.MachineId == id);
      return View(thisEngineer);
    }

    public ActionResult Delete(int id)
    {
      var thisEngineer = _db.Machines.FirstOrDefault(Machine => Machine.MachineId == id);
      return View(thisEngineer);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisEngineer = _db.Machines.FirstOrDefault(Machine => Machine.MachineId == id);
      _db.Machines.Remove(thisEngineer);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

        public ActionResult AddMachine(int id)
    {
      var thisEngineer = _db.Engineers.FirstOrDefault(Engineer => Engineer.EngineerId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
      return View(thisEngineer);
    }

    [HttpPost]
    public ActionResult AddEngineer(Machine machine, int EngineerId)
    {
      if (EngineerId != 0)
      {
        _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId });
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
  }
}