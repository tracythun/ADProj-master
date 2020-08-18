﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ADProj.Services;
using ADProj.Models;
using Microsoft.AspNetCore.Http;

namespace ADProj.Controllers
{
    public class RetrievalController : Controller
    {
        private RetrievalService rs;

        public RetrievalController(RetrievalService rs)
        {
            this.rs = rs;
        }
        public IActionResult Index()
        {
            Dictionary<string, int> retrieveList = new Dictionary<string, int>();
            List<Request> requests = rs.RetrieveApprovedRequest();
            int empId = Convert.ToInt32(HttpContext.Session.GetString("id"));
            foreach (Request r in requests)
            {
                List<RequestDetails> rds = rs.FindRquestDetailsById(r.Id);
                foreach(RequestDetails rd in rds)
                {
                    if (retrieveList.ContainsKey(rd.InventoryItemId))
                    {
                        int currqty = retrieveList[rd.InventoryItemId];
                        int updateqty = currqty + rd.QtyRequested;
                        retrieveList[rd.InventoryItemId] = updateqty;
                    }
                    else
                    {
                        retrieveList[rd.InventoryItemId] = rd.QtyRequested;
                    }
                }

                int retId = rs.CreateRetrieval(empId, requests);
                rs.CreateRetrievalDetails(retId, retrieveList);
                List<RetrievalDetails> rtList = rs.FindRetrievalDetails(retId);
                ViewData["retrieveList"] = rtList;
                ViewData["retId"] = retId;
            }
            return View();
        }
    }
}