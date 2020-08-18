﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADProj.Models;
using ADProj.Services;
using Microsoft.AspNetCore.Mvc;

namespace ADProj.Controllers
{
    public class SupplierController : Controller
    {
        private SupplierService supService;

        public SupplierController(SupplierService supService)
        {
            this.supService = supService;
        }
        public IActionResult Index()
        {
            List<Supplier> supplierList = supService.SupplierList();
            ViewData["supplierList"] = supplierList;
            return View();
        }

        public IActionResult AddSupplier(string id)
        {
            return View();
        }

        public IActionResult AddStationery(string id)
        {

            SupplierStationery s = supService.GetSupplierStationeryBySupplierId(id);
            ViewData["supplierid"] = id;
            ViewData["alertMsg"] = TempData["alreadyExistStationary"];
            return View();
        }

        public IActionResult SaveSupplier(string Id, string Name, string ContactName, string PhoneNo, string FaxNo, string Address, string GSTReg)
        {

            if (!(Id != null && Name != null && ContactName != null && PhoneNo != null && FaxNo != null && Address != null && GSTReg != null))
            {
                TempData["alertMsg"] = "Please enter all information!";
                return RedirectToAction("AddSupplier");
            }
            else
            {
                supService.CreateSupplier(Id, Name, ContactName, PhoneNo, FaxNo, Address, GSTReg);
                TempData["alertMsg"] = "Saved successfully!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult SaveStationery(int Id, string SupplierId, string InventoryItemId, string UOM, float TenderPrice)
        {
            List<SupplierStationery> supplierStationeryList = supService.GetSupplierStationeryListByCompositeKey(SupplierId, InventoryItemId);
            if (supplierStationeryList == null || supplierStationeryList.Count == 0)
            {
                if (!(SupplierId != null && InventoryItemId != null && UOM != null))
                {
                    TempData["alertMsg"] = "Please enter all information!";
                    return RedirectToAction("AddStationery");
                }
                else
                {
                    supService.CreateSupplierStationery(SupplierId, InventoryItemId, UOM, TenderPrice);
                    TempData["alertMsg"] = "Saved successfully!";
                    return RedirectToAction("Details", new { Id = SupplierId });
                }
            }
            else
            {
                TempData["alreadyExistStationary"] = "Already Exist!";
                return RedirectToAction("AddStationery", new { id = SupplierId });
            }
        }

        public IActionResult SupplierList()
        {
            List<Supplier> supplierList = supService.SupplierList();
            ViewData["SupplierList"] = supplierList;
            return View();
        }
        public IActionResult EditDeleteSupplier(string cmd, string Id)
        {
            if (cmd == "delete")
            {
                supService.DeleteSupplierById(Id);
                return RedirectToAction("Index");
            }
            if (cmd == "edit")
            {
                Supplier supplier = supService.GetSupplierById(Id);
                ViewData["item"] = supplier;
                return View("UpdateSupplier");
            }
            return RedirectToAction("Index");
        }

        public IActionResult EditDeleteStationery(string? cmd, int Id) //stationery id
        {
            SupplierStationery supplierstationery = supService.GetSupplierStationeryById(Id);
            if (cmd == "delete")
            {
                supService.DeleteSupplierStationeryById(Id);
                return RedirectToAction("Details", new { Id = supplierstationery.SupplierId });
            }
            if (cmd == "edit")
            {

                ViewData["item"] = supplierstationery;

                return View("UpdateStationery");
            }

            SupplierStationery s = supService.GetSupplierStationeryById(Id);
            return RedirectToAction("Details", new { Id = s.SupplierId });
        }

        public IActionResult UpdateSupplier(string Id, string Name, string ContactName, string PhoneNo, string FaxNo, string Address, string GSTReg)
        {
            supService.UpdateSupplierById(Id, Name, ContactName, PhoneNo, FaxNo, Address, GSTReg);

            return RedirectToAction("Index");
        }

        public IActionResult UpdateStationery(int StationeryId, string SupplierId, string InventoryItemId, string UOM, float TenderPrice)
        {
            supService.UpdateSupplierStationeryById(SupplierId, InventoryItemId, UOM, TenderPrice);
            SupplierStationery s = supService.GetSupplierStationeryById(StationeryId);
            /*return RedirectToAction("EditDeleteStationery", new { Id = s.Id });*/
            return RedirectToAction("Details", new { Id = SupplierId });
        }

        public IActionResult Details(string Id)
        {
            List<SupplierStationery> supplierstationeryList = supService.GetSupplierStationeryListById(Id);
            ViewData["supplierstationeryList"] = supplierstationeryList;
            ViewData["supplierid"] = Id;

            return View();
        }
    }
}