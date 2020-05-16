using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoAndFind.Wiew
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class explains : ContentPage
    {


        public explains()
        {
            Label header = new Label
            {
                Text = "ListView",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            List<ItemExplain> ItemExplain = new List<ItemExplain>
            {
                new ItemExplain("Liquor","Liquor is an item which regenerates 1 health when it is used, it can not increase your maximal health ammount"),
                new ItemExplain("Armour","Armour is an item which regenerates 1 health and also increases your maximal health ammount by 1"),
                new ItemExplain("Veteran Bandit","Veteran Bandit is a creature which can damage you by 2 after losing a minigame (if you win a minigame there is a chance to play again if bandit wants)"),
                new ItemExplain("Causual Bandit","Causual Bandit is a creature which can damage you by 1 after losing a minigame"),
                new ItemExplain("Piece of the world","Piece of the world is an item which creates a circular area in which legendary ItemExplain can be found, if there are not any legendary ItemExplain, it will generate it"),
                new ItemExplain("Hopefull stick of gloominess","Hopefull stick of gloominess is an item which completly regenerates all ItemExplain"),
                new ItemExplain("Dead man's macaroni","Dead man's macaroni is an item which completely regenerates your health and it can also save you from death"),
                new ItemExplain("Erasing wand","If you met a bandit, you can easily erase him by using Erasing wand"),
            };
            ListView listView = new ListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Label nameLabel = new Label();
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    Label explainLabel = new Label();
                    explainLabel.SetBinding(Label.TextProperty, "Explain");
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new StackLayout
                                {
                                    VerticalOptions = LayoutOptions.Center,
                                    Spacing = 0,
                                    Children =
                                    {
                                        nameLabel,
                                        explainLabel
                                    }
                                }
                            }
                        }
                    };

                })
            };
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            this.Content = new StackLayout
            {
                Children =
                {
                    header,
                    listView
                }
            };
        }       
    }
}




    public class ItemExplain
    {
        public ItemExplain(string explain, string name)
        {
            Name = name;
            Explain = explain;
        }
        public string Name { get;  private set; }
        public string Explain { get; private set; }
    }

