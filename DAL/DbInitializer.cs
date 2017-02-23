using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DAL
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            Page p1 = new Page
            {
                Url = "ergfresgfsergse",
                Ping = 12,
                Size = 23
            };
            Site s1 = new Site
            {
                LastUpdate = DateTime.Now,
                Url = "erferfqefrga",
                Pages = new List<Page> { p1 }
            };
            context.Sites.Add(s1);
            context.SaveChanges();
        }
    }
}