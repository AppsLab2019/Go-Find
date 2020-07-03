using Android.Content.Res;
using Android.Views;
using GoAndFind.viewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoAndFind
{
    class MiniGame
    {
        public bool Win { get; private set; }
        public bool ErasingWandUsed { get; private set; }
        public async Task<bool> Fight( string bandit, Player player)
        {
            Win = false;
            var rand = new Random();
            var RockETC = new List<string> { "sword", "shield", "net" };
            if (player.Inventory.Contains("Erasing wand"))
            {
                bool decision = await App.Current.MainPage.DisplayAlert(null, "You can remove this bandit by using Erasing wand", "Remove Bandit", "Fight");
                if (decision)
                {
                    Win = true;
                    ErasingWandUsed = true;
                    player.Inventory.Remove("Erasing wand");
                    return ErasingWandUsed;
                    
                }
            } 
            int b = 1;
            for (int a = 0; a < b; a++)
            {
                string BanditAction = RockETC[rand.Next(0, RockETC.Count)];
                var playerAction = await App.Current.MainPage.DisplayActionSheet("Choose ", null, null, "sword", "shield", "net");
                await App.Current.MainPage.DisplayAlert(null, "Bandit used " + BanditAction, "ok");
                if (playerAction == null)
                {
                    b++;
                    break;
                }
                if (BanditAction == playerAction)
                {
                    b++;
                    await App.Current.MainPage.DisplayAlert(null, "You both used " + playerAction, "continue fight");
                }
                if (BanditAction == "sword")
                {
                    if (playerAction.ToString() == "shield")
                        Win = true;

                    else
                        Win = false;
                }
                if (BanditAction == "shield")
                {
                    if (playerAction.ToString() == "net")
                        Win = true;
                    else
                        Win = false;
                }

                if (BanditAction == "net")
                {
                    if (playerAction.ToString() == "sword")
                        Win = true;
                    else
                        Win = false;
                }
                if (bandit.Contains("Veteran") && rand.Next(0, 100) < 30 && Win == true)
                {
                    await App.Current.MainPage.DisplayAlert(null, "Veteran gone even more angry, try to beat him again", "ok");
                    b++;
                }
                if (Win == true)
                {
                    await App.Current.MainPage.DisplayAlert(null, "Ho, Ho, Ho ... You won this fight, now let's continue ", "ok");
                }
                else if(BanditAction != playerAction)
                {
                    await App.Current.MainPage.DisplayAlert(null, "The bandit beat you", "ouch");
                }
            }
            return Win;
        }
        public async Task<Player>Ambush(Item item,Player player)
        {
            var rnd = new Random();
            bool friend = false;
            bool gift = false;
            var fight = new MiniGame();
            await App.Current.MainPage.DisplayAlert("Watchout!", "You are ambushed by " + item.Ammount + " " + item.Name, "OK");
            if (player.Inventory.Contains("Liquor") && item.Name.Contains("Causual"))
            {
                gift = await App.Current.MainPage.DisplayAlert("Question?", "Those bandits look friendly, we may be friends", "Offer liquor", "Fight");
                if (gift)
                {
                    player.Inventory.Remove("Liquor");
                    if (rnd.Next(0, 100) < 30)
                    {
                        friend = true;
                        await App.Current.MainPage.DisplayAlert(null, "Looks like your gift was accepted", "ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(null, "Your liquor was accepted but you are being attacked anyway... ", "My liquor!");
                    }
                }

            }
            if (!friend)
            {
                for (int a = 0; a < item.Ammount; a++)
                {
                    await Fight(item.Name, player);
                    if (!Win)
                    {
                        player.Hurt(1);
                    }
                    else if (gift && rnd.Next(0, 100) < 40)
                    {
                        player.Inventory.Add("Liquor");
                        gift = false;
                        await App.Current.MainPage.DisplayAlert("Liquor", "As a bandit was running away , he dropped your liquor ", "Hope he will remember this");
                    }
                }
            }
            else
                player.Inventory.Remove("liquor");
            return player;
        }
    }
}
