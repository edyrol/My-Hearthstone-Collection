using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthMirror;
using HearthDb;


namespace HearthstoneCollectionExporter
{
    static class HearthstoneCollectionExporter
    {
        [STAThread]
        static void Main()
        {
            var status = HearthMirror.Status.GetStatus().MirrorStatus;
            if (status == HearthMirror.Enums.MirrorStatus.Ok)
            {
                //Standard sets
                var StandardSets = new List<string> { "CORE", "EXPERT1", "BOOMSDAY", "TROLL", "DALARAN" };
                //Get collection from Hearthstone
                var goldenCollection = Reflection.GetCollection().Where(x => x.Premium);
                var commonCollection = Reflection.GetCollection().Where(x => !x.Premium);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("data\\MyCollection.txt"))
                {
                    foreach (var set in StandardSets)
                    {
                        foreach (var dbCard in Cards.Collectible.Where(x => x.Value.Set.ToString() == set))
                        {
                            //Count hoy many copies of the card you have
                            int amount = commonCollection.Where(x => x.Id.Equals(dbCard.Key)).Select(x => x.Count).FirstOrDefault() + goldenCollection.Where(x => x.Id.Equals(dbCard.Key)).Select(x => x.Count).FirstOrDefault();
                            if (amount >= 1)
                            {
                                //Decide how many cards you practically have
                                if (dbCard.Value.Rarity.ToString() == "LEGENDARY")
                                    amount = Math.Min(amount, 1);
                                else
                                    amount = Math.Min(amount, 2);
                                //Print name of card and number of copies
                                //Console.WriteLine(dbCard.Value.Name + " " + amount);
                                file.WriteLine(dbCard.Value.Name + " " + amount);
                            }
                        }
                    }
                }
            }
            else if (status == HearthMirror.Enums.MirrorStatus.ProcNotFound)
            {
                Console.WriteLine("Hearthstone not open");
            }
            else if (status == HearthMirror.Enums.MirrorStatus.Error)
            {
                Console.WriteLine("ERROR");
            }
        }
    }
}
