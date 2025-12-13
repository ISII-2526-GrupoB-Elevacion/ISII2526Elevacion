using AppForSEII2526.API.Models;
using Humanizer.Localisation;

namespace AppForSEII2526.API.Data
{
    public static class SeedData
    {

        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider, ILogger logger)
        {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            try
            {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the roles in the Database.");
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try
            {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Users in the Database.");
            }

            try
            {
                SeedModelsAndCars(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Cars and Models in the Database.");
            }

            try
            {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "elena@uclm.es");
                SeedPurchase(dbContext, user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding a Purchase in the Database.");
            }

            try
            {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "elena@uclm.es");
                SeedRental(dbContext, user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding a Rental in the Database.");
            }

            try
            {
                var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "elena@uclm.es");
                SeedReview(dbContext, user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding a Review in the Database.");
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {

            foreach (string roleName in roles)
            {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, List<string> roles)
        {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("gregorio@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("2", "Gregorio", "Diaz Descalzo", "gregorio@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "APassword1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                }
            }

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null)
            {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                ApplicationUser user = new ApplicationUser("3", "Peter", "Jackson", "peter@uclm.es");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();

                }
            }
        }

        public static void SeedModelsAndCars(ApplicationDbContext dbcontext)
        {
            string[] modelsnames = ["Audi A4", "Toyota Corolla", "Renault Clio"];
            List<Model> models = [];
            Car car;
            foreach (string modelname in modelsnames)
            {
                var model = dbcontext.Model.FirstOrDefault(g => g.Name == modelname);
                if (model == null)
                    models.Add(new Model(modelname));
                else
                    models.Add(model);
            }
            if (dbcontext.Car.FirstOrDefault(c => c.Model.Name == "Audi A4") == null)
            {
                Model model = new Model("Audi A4");
                car = new Car("Berlina", "Gris", "Automovil de turismo del segmento D", "Audi", 18000f,3,1,85f,"150 CV","Gasolina", "Aceite y ruedas", 59.5f, model);
                dbcontext.Car.Add(car);
            }

            if (dbcontext.Car.FirstOrDefault(c => c.Model.Name == "Toyota Corolla") == null)
            {
                Model model = new Model("Toyota Corolla");
                car = new Car("Familiar", "Rojo", "Automovil de turismo del segmento C", "Toyota", 5000f, 10, 7, 60f, "80 CV", "Diesel", "Líquido refrigerante", 45.6f, model);
                dbcontext.Car.Add(car);
            }

            dbcontext.SaveChanges();
        }


        public static void SeedPurchase(ApplicationDbContext dbcontext, ApplicationUser user)
        {

            if (dbcontext.Purchase.FirstOrDefault(p => p.Id == 1) == null)
            {
                var purchase = new Purchase("C/Prim, 7", Purchase.PurchasePaymentMethodEnum.Visa, DateTime.Today, new List<PurchaseItem>(), user);
                purchase.PurchaseItems.Add(new PurchaseItem(1,1,purchase));
                dbcontext.Purchase.Add(purchase);
            }
            dbcontext.SaveChanges();
        }


        public static void SeedRental(ApplicationDbContext dbcontext, ApplicationUser user)
        {
            if (dbcontext.Rental.FirstOrDefault(p => p.Id == 1) == null)
            {
                var rental = new Rental("C/Prim, 7", DateTime.Today.AddDays(5), Rental.RentalPaymentMethodEnum.Visa, DateTime.Today.AddDays(1), DateTime.Today, new List<RentalItem>(), user);
                rental.RentalItems.Add(new RentalItem(1,1,rental));
                dbcontext.Rental.Add(rental);
            }
            dbcontext.SaveChanges();
        }

        public static void SeedReview(ApplicationDbContext dbcontext, ApplicationUser user)
        {
            if (dbcontext.Review.FirstOrDefault(p => p.Id == 1) == null)
            {
                var review = new Review("Spain", DateTime.Today, "Novato", new List<ReviewItem>(), user);
                review.ReviewItems.Add(new ReviewItem(1, "Muy bonito", 3.5f, review));
                dbcontext.Review.Add(review);
            }
            dbcontext.SaveChanges();
        }
    }
}