using Android.Content.Res;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace GoAndFind
{
    class MiniGame
    {
        public bool Win;
        public async void Fight(MainPage page, string bandit)
        {
            Win = false;
            var rand = new Random();
            var RockETC = new List<string> { "sword", "shield", "net" };
            string BanditAction = RockETC[rand.Next(1, RockETC.Count)];

            int b = 1;
            for (int a = 0; a < b; a++)
            {
                var PLayerAction = await page.DisplayActionSheet("Choose ", null, null, "sword", "shield", "net");
                await page.DisplayAlert(null, "Bandit used " + BanditAction, "ok");
                if (BanditAction == PLayerAction.ToString())
                {
                    b++;
                }
                if (BanditAction == "sword")
                {
                    if (PLayerAction.ToString() == "shield")
                        Win = true;

                    else
                        Win = false;
                }
                if (BanditAction == "shield")
                {
                    if (PLayerAction.ToString() == "net")
                        Win = true;
                    else
                        Win = false;
                }

                if (BanditAction == "net")
                {
                    if (PLayerAction.ToString() == "sword")
                        Win = true;
                    else
                        Win = false;
                }
                if (bandit == "veteran" && rand.Next(0, 100) < 30 && Win == true)
                {
                    await page.DisplayAlert(null, "Veteran has gone even more angry, try to beat him again", "ok");
                    b++;
                }
                if (Win == true)
                {
                    page.DisplayAlert(null, "Ho, Ho, Ho ... You've won this fight, now let's continue ", "ok");
                }
                else
                {
                    page.DisplayAlert(null, "Bandit has beaten you", "ouch");
                }
            }
        }
    }
}
