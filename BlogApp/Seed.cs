using BlogApp.Data;
using BlogApp.Models;

namespace BlogApp
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }

        public void SeedDataContext()
        {
            if (!dataContext.Blogs.Any())
            {
                var Authors = new List<Author>()
                {
                    new Author()
                    {
                        Blogs = new List<Blog>()
                        {
                           new Blog()
                           {
                               BlogTags = new List<BlogTag>()
                               {
                                   new BlogTag()
                                   {
                                       Tag = new Tag()
                                       {
                                           TagName = "doNet"
                                       }
                                   }
                               },
                               comments = new List<Comment>()
                               {
                                   new Comment()
                                   {
                                       User = new User(){ FirstName = "ali", LastName = "ahmad", Email = "Ali@gmail.com", Password="asdas1212"} ,
                                       Text = " This is comment from Ali",
                                       date = DateTime.Now,
                                   }
                               },
                               title = "Blog1",
                               description = "descroption for blog1",
                               body = " a long text will be here for body!",
                               createdAt = new DateTime(2022,2,5),
                               updatedAt = new DateTime(),
                           }
                        },
                        FirstName = "Mohsen",
                        LastName = "Ahmadi",
                        Email = "mohen@gmail.com",
                        Password = "adlaksdlkals",
                        UserName = "gihev",
                        ProfileImage="https://fksdjf.df"
                    },

                    new Author()
                    {
                        Blogs = new List<Blog>()
                        {
                           new Blog()
                           {
                               BlogTags = new List<BlogTag>()
                               {
                                   new BlogTag()
                                   {
                                       Tag = new Tag()
                                       {
                                           TagName = "react"
                                       }
                                   }
                               },
                               comments = new List<Comment>()
                               {
                                   new Comment()
                                   {
                                       User = new User(){ FirstName = "omid", LastName = "aram", Email = "omid@gmail.com", Password="6565jhjh"} ,
                                       Text = " This is comment from omid",
                                       date = DateTime.Now,
                                   }
                               },
                               title = "Blog2",
                               description = "descroption for blog2",
                               body = " a long text will be here for body 33",
                               createdAt = new DateTime(2022,2,6),
                               updatedAt = new DateTime(),
                           }
                        },
                        FirstName = "Homeira",
                        LastName = "Khadem",
                        Email = "homeira@gmail.com",
                        Password = "56522klklhj",
                        UserName = "klo",
                        ProfileImage="https://kloo.df"
                    },


                };

                dataContext.Author.AddRange(Authors);
                dataContext.SaveChanges();
            }
            

        }
    }
}
