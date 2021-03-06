﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Data {
    public static class ProjectDBContextExtensions {

        private static List<Subject> _subject = new List<Subject> {
            new Subject { SubjectName = "Dutch", Description="Deze Quiz is in het Nederlands"},
            new Subject { SubjectName = "English", Description="This Quiz is in English"},
            new Subject { SubjectName = "Hardware", Description="Want to test your knowledge about Hardware? Go and take on this quiz"},
        };

        private static List<Question> _question = new List<Question> {
            new Question { Questiontext = "Wat is de hoofdstad van België", Score=1},
            new Question { Questiontext = "Wat is de hoofdstad van Duitsland", Score=1},
            new Question { Questiontext = "Wat is de hoofdstad van West-Vlaanderen", Score=1},
        };

        public async static Task SeedRoles(RoleManager<IdentityRole> RoleMgr) {
            IdentityResult roleResult;
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames) {
                var roleExist = await RoleMgr.RoleExistsAsync(roleName);
                if (!roleExist) {
                    roleResult = await RoleMgr.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public async static Task SeedUsers(UserManager<IdentityUser> userMgr) {
            //1. Admin aanmaken ---------------------------------------------------
            if (await userMgr.FindByNameAsync("Docent@MCT") == null)  //controleer de UserName
            {
                var user = new IdentityUser() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Docent@MCT",
                    Email = "Docent@MCT",
                };

                var userResult = await userMgr.CreateAsync(user, "Docent@1");
                var roleResult = await userMgr.AddToRoleAsync(user, "Admin");
                // var claimResult = await userMgr.AddClaimAsync(user, new Claim("DocentWeb", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded) {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
            //2. meerdere users  aanmaken --------------------------------------------
            //2a. persons met rol "user" aanmaken
            var nmbrUsers = 9;
            for (var i = 1; i <= nmbrUsers; i++) {
                if (userMgr.FindByNameAsync("Student" + i).Result == null) {
                    IdentityUser user = new IdentityUser {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "Student" + i,
                        Email = "emailSt" + i + "@howest.be",
                    };

                    var userResult = await userMgr.CreateAsync(user, "studentP@sw00rd" + i);
                    var roleResult = await userMgr.AddToRoleAsync(user, "User");
                    if (!userResult.Succeeded || !roleResult.Succeeded) {
                        throw new InvalidOperationException("Failed to build " + user.UserName);
                    }
                }
            }
        }

        public async static Task SeedData(this ProjectDbContext context) {

            //1. Subjects aanvullen met hardcoded data -----------------------------------
            if (!context.Subject.Any()) {
                Debug.WriteLine("Seeding Subjects");
                foreach (Subject e in _subject) {
                    if (!context.Subject.Any(s => s.Id == e.Id))
                        await context.Subject.AddAsync(e);
                }
                await context.SaveChangesAsync();
            }

            //1. Questions aanvullen met hardcoded data -----------------------------------
            if (!context.Question.Any()) {
                Debug.WriteLine("Seeding Questions");
                foreach (Question e in _question) {
                    if (!context.Question.Any(s => s.Id == e.Id))
                        await context.Question.AddAsync(e);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
