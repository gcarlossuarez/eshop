using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using catalog.api.Models;
using System.Security.Cryptography.X509Certificates;

namespace eshop.webapp
{
    public class ProductsController : Controller
    {

        // ENUMERATIONS

        private enum ETypeError
        {
            // range of 10, between each type of error; in case, later on, you need to enter intermediate values
            eNothing = 0,

            eProductIdOutOfValidRange = 10,

            eIdRepeated = 20,

            eNullProductDescription = 30,

            eNullProductBrand = 40,

            eNullProductModel = 50,

            eNullProductStatus = 60,

            eUnitPriceLessThanOrEqualToZero  = 70,

            eDiscountPercentageLessThanMinimum  = 80,
            eDiscountPercentageExceedsTheMaximumValue = 90,

            eStatusValueIsInvalid = 100
        }



        // CONSTANTS

        private const int cMinimumDiscountPercentage = 1;
        private const int cMaximumDiscountPercentage = 99;

        // PROPERTYS

        private readonly eshopContext _context = new eshopContext();
        // In case, later, the possible states will increase; only, one more element must be added to this "List"
        private List<string> _ListStatusValue = new List<string>() { "C", "E" };


        // METHODS

        //public ProductsController(eshopContext context)
        //{
        //    _context = context;
        //}
        public ProductsController()
        {
            //_context = webapp.Controllers.HomeController._eshopContext;
        }


        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var l_CProduct = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (l_CProduct == null)
            {
                return NotFound();
            }

            return View(l_CProduct);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var l_CProduct = _context.Product.OrderByDescending(u => u.Id).FirstOrDefault();
            if (l_CProduct == null)
            {
                l_CProduct.Id = 1;
            }
            else
            {
                l_CProduct.Id += 1;
            }
            l_CProduct.Description = "";
            l_CProduct.Brand = "";
            l_CProduct.Model = "";
            l_CProduct.UnitPrice = 0;
            l_CProduct.DiscountPercentage = 0;
            l_CProduct.ProductStatus = "C";
            return View(l_CProduct);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Brand,Model,UnitPrice,DiscountPercentage,ProductStatus")] Product p_CProduct)
        {
            if (ModelState.IsValid)
            {
                p_CProduct = FormatProduct(p_CProduct);
                CGeneratedError l_CGeneratedError = new CGeneratedError((short)ProductsController.ETypeError.eNothing, "");
                if (NewProductIsValid(p_CProduct, ref l_CGeneratedError))
                {
                    _context.Add(p_CProduct);
                    await _context.SaveChangesAsync();

                    // It is only a warning. Not critical. You can comment later on.
                    if (HomonymExists(p_CProduct))
                    {
                        return View("WarningDisplay", "New product added successfully; but, keep in mind that there is already another product with the same description");
                    }
                }
                else
                {
                    return View("ErrorDisplay", l_CGeneratedError);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(p_CProduct);
        }

        bool HomonymExists(Product p_CProduct)
        {
            bool l_HomonymExists = false;

            if(_context.Product.Where(e => e.Description.Trim() == p_CProduct.Description).Count() > 1)
            {
                l_HomonymExists = true;
            }

            return l_HomonymExists;
        }

        private bool NewProductIsValid(Product p_CProduct, ref CGeneratedError p_CGeneratedError)
        {
            bool l_NewProductIsValid = true;


            p_CGeneratedError = new CGeneratedError((short)ProductsController.ETypeError.eNothing, "");

            if (p_CProduct.Id < 0)
            {
                p_CGeneratedError.Id = (short)ProductsController.ETypeError.eProductIdOutOfValidRange;
                p_CGeneratedError.Description = "Product Id., Out of valid range";
            }
            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                if (true == ProductExists(p_CProduct.Id) )
                {
                    p_CGeneratedError.Id = (short)ProductsController.ETypeError.eIdRepeated;
                    p_CGeneratedError.Description = "Product ID already used in another product";
                }
            }

            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                if( true == BasicDataAreValid(p_CProduct, ref  p_CGeneratedError) )
                {

                }
            }

            if (p_CGeneratedError.Id != (short)ProductsController.ETypeError.eNothing)
            {
                l_NewProductIsValid = false;
            }

            return l_NewProductIsValid;
        }

        bool BasicDataAreValid(Product p_CProducto, ref CGeneratedError p_CGeneratedError)
        {
            bool l_BasicDataAreValid = true;

            p_CGeneratedError = new CGeneratedError((short)ProductsController.ETypeError.eNothing, "");


            /////////
            if(p_CProducto.Description.Trim().Length == 0)
            {
                p_CGeneratedError.Id = (short)ProductsController.ETypeError.eNullProductDescription;
                p_CGeneratedError.Description = "Null product description";
            }


            //////////
            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                if (p_CProducto.Brand.Trim().Length == 0)
                {
                    p_CGeneratedError.Id = (short)ProductsController.ETypeError.eNullProductBrand;
                    p_CGeneratedError.Description = "Null product brand";
                }
            }


            /////////
            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                if (p_CProducto.Model.Trim().Length == 0)
                {
                    p_CGeneratedError.Id = (short)ProductsController.ETypeError.eNullProductModel;
                    p_CGeneratedError.Description = "Null product model";
                }
            }


            ////////
            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                if (p_CProducto.ProductStatus.Trim().Length == 0)
                {
                    p_CGeneratedError.Id = (short)ProductsController.ETypeError.eNullProductStatus;
                    p_CGeneratedError.Description = "Null product status";
                }
            }


            ///////
            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                if (p_CProducto.UnitPrice <= 0)
                {
                    p_CGeneratedError.Id = (short)ProductsController.ETypeError.eUnitPriceLessThanOrEqualToZero;
                    p_CGeneratedError.Description = "Unit price less than or equal to zero";
                }
            }


            /////////
            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                if (p_CProducto.DiscountPercentage < ProductsController.cMinimumDiscountPercentage)
                {
                    p_CGeneratedError.Id = (short)ProductsController.ETypeError.eDiscountPercentageLessThanMinimum;
                    p_CGeneratedError.Description = "Discount percentage less than minimum value '" +
                                                    ProductsController.cMinimumDiscountPercentage.ToString() + "'";
                }
            }

            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                if (p_CProducto.DiscountPercentage > ProductsController.cMaximumDiscountPercentage)
                {
                    p_CGeneratedError.Id = (short)ProductsController.ETypeError.eDiscountPercentageExceedsTheMaximumValue;
                    p_CGeneratedError.Description = "Discount percentage exceeds the maximum value '" + 
                                                    ProductsController.cMaximumDiscountPercentage.ToString() + "'";
                }
            }
            

            /////////
            if (p_CGeneratedError.Id == (short)ProductsController.ETypeError.eNothing)
            {
                //if ( ( p_CProducto.ProductStatus != "C") && (p_CProducto.ProductStatus != "E") )
                if( _ListStatusValue.Where(x=> x.Trim() == p_CProducto.ProductStatus).Count() == 0 )
                {
                    p_CGeneratedError.Id = (short)ProductsController.ETypeError.eStatusValueIsInvalid;
                    p_CGeneratedError.Description = "Status value is invalid. It should be ";
                    foreach(var l_StatusValue in _ListStatusValue)
                    {
                        p_CGeneratedError.Description += "'" + l_StatusValue + "' ";
                    }
                }
            }


            ////////
            if (p_CGeneratedError.Id != (short)ProductsController.ETypeError.eNothing)
            {
                l_BasicDataAreValid = false;
            }

            return l_BasicDataAreValid;
        }

        public Product FormatProduct(Product p_CProduct)
        {
            // in case there were null values
            p_CProduct.Description ??= "";
            p_CProduct.Brand ??= "";
            p_CProduct.Model ??= "";
            p_CProduct.ProductStatus ??= "";


            // The null does not need to be validated; since the form validates it;
            // also, according to c #, it cannot be null, it should be "0", in the but
            // of the cases; unless, the value is nullable
            //p_CProduct.UnitPrice == null ? 0: p_CProduct.UnitPrice;
            //p_CProduct.DiscountPercentage == null? 0: p_CProduct.DiscountPercentage;

            // blanks on the left and / or right are removed
            p_CProduct.Description = p_CProduct.Description.Trim().ToUpper();
            p_CProduct.Brand = p_CProduct.Brand.Trim().ToUpper();
            p_CProduct.Model = p_CProduct.Model.Trim().ToUpper();
            p_CProduct.ProductStatus = p_CProduct.ProductStatus.Trim().ToUpper();

            return p_CProduct;
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Brand,Model,UnitPrice,DiscountPercentage,ProductStatus")] Product p_CProduct)
        {
            if (id != p_CProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                p_CProduct = FormatProduct(p_CProduct);
                CGeneratedError l_CGeneratedError = new CGeneratedError((short)ProductsController.ETypeError.eNothing, "");
                if (BasicDataAreValid(p_CProduct, ref l_CGeneratedError))
                {
                    try
                    {
                        _context.Update(p_CProduct);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(p_CProduct.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    // It is only a warning. Not critical.You can comment later on.
                    if (HomonymExists(p_CProduct))
                    {
                        return View("WarningDisplay", "Product successfully modified; but, keep in mind that there is already another product with the same description");
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("ErrorDisplay", l_CGeneratedError);
                }
            }
            return View(p_CProduct);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
