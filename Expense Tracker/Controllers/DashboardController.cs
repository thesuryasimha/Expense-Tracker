using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            //Last 7 days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today.AddDays(1);

            
                List<Transaction> SelectedTransactions = await _context.Transactions
                        .Include(x => x.Category)
                        .Where(y => y.DateTime >= StartDate && y.DateTime <= EndDate)
                        .ToListAsync();
                //Total Income
                decimal TotalIncome = SelectedTransactions
                    .Where(i => i.Category.Type == "Income")
                    .Sum(j => j.Amount);
                ViewBag.TotalIncome = TotalIncome.ToString("C0");

                // Total Expense
                decimal TotalExpense = SelectedTransactions
                    .Where(i => i.Category.Type == "Expense")
                    .Sum(j => j.Amount);
                ViewBag.TotalExpense = TotalExpense.ToString("C0");


                //Balance
                decimal Balance = TotalIncome - TotalExpense;
                ViewBag.Balance = Balance.ToString("C0");
                ViewData["Balance"]= Balance.ToString("C0");


            


            return View();
        }
    }
}
