using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adultus.Helpers;
using Adultus.Models;

namespace Adultus.ViewModels
{
    public class LayoutViewModel
    {
        public Users User { get; set; }

        public List<Users> Users { get; set; }

        public List<Roles> Roles { get; set; }

        public List<Profiles> Profiles { get; set; }

        public List<ProfileRoles> ProfileRoles { get; set; }

        public LayoutViewModel LayoutViewModelBuilder(string profileId, string userId)
        {
            SqlHelper.DbContext();
            //Using aspnetclaim rules to store roles which have been manually inputted
            //NOT ADDED BY IDS
            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(profileId);

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            List<Users> users = new List<Users>();
            users = SqlHelper.GetAllUsers();

            List<Roles> roles = new List<Roles>();
            roles = SqlHelper.GetAllRoles();

            List<Profiles> profiles = new List<Profiles>();
            Profiles p = new Profiles();
            p = SqlHelper.GetProfile(profileId);
            profiles.Add(p);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                User = user,
                Users = users,
                Profiles = profiles,
                Roles = roles
            };
        }

        public LayoutViewModel LayoutViewModelUserBuilder(string profileId, string userId)
        {
            SqlHelper.DbContext();
            //Using aspnetclaim rules to store roles which have been manually inputted
            //NOT ADDED BY IDS
            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(profileId);

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            List<Profiles> profiles = new List<Profiles>();
            Profiles p = new Profiles();
            p = SqlHelper.GetProfile(profileId);
            profiles.Add(p);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                User = user,
                Profiles = profiles
            };
        }

        public LayoutViewModel LayoutViewModelUsersBuilder(string profileId, string userId)
        {
            SqlHelper.DbContext();
            //Using aspnetclaim rules to store roles which have been manually inputted
            //NOT ADDED BY IDS
            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(profileId);

            List<Users> users = new List<Users>();
            users = SqlHelper.GetAllUsers();

            List<Roles> roles = new List<Roles>();
            roles = SqlHelper.GetAllRoles();

            List<Profiles> profiles = new List<Profiles>();
            Profiles p = new Profiles();
            p = SqlHelper.GetProfile(profileId);
            profiles.Add(p);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                Users = users,
                Profiles = profiles,
                Roles = roles
            };
        }

        public LayoutViewModel LayoutViewModelProfileRolesBuilder(string profileId, string userId)
        {
            SqlHelper.DbContext();
            //Using aspnetclaim rules to store roles which have been manually inputted
            //NOT ADDED BY IDS
            List<ProfileRoles> profileRoles = new List<ProfileRoles>();
            profileRoles = SqlHelper.GetProfileRoles(profileId);

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            return new LayoutViewModel
            {
                ProfileRoles = profileRoles,
                User = user
            };
        }

        public LayoutViewModel LayoutViewModelRolesBuilder(string userId)
        {
            SqlHelper.DbContext();

            Users user = new Users();
            user = SqlHelper.GetUser(userId);

            List<Roles> roles = new List<Roles>();
            roles = SqlHelper.GetAllRoles();

            return new LayoutViewModel
            {
                User = user,
                Roles = roles
            };
        }

        public LayoutViewModel LayoutViewModelAllProfilesBuilder(string userId)
        {
            SqlHelper.DbContext();

            Users user = new Users();
            user = SqlHelper.GetUser(userId);
            
            List<Profiles> profiles = new List<Profiles>();
            profiles = SqlHelper.GetAllProfiles();

            return new LayoutViewModel
            {
                User = user,
                Profiles = profiles
            };
        }
    }
}