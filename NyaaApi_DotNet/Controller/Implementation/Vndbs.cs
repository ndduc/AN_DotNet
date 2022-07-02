using System;
using VndbSharp;
using VndbSharp.Models;




namespace NyaaApi_DotNet.Controller.Implementation
{
    public class Vndbs
    {
        dynamic _client;
        public void test()
        {

            Vndb v = new Vndb();
            /*            var test = v.GetVisualNovelAsync(VndbFilters.Search.Fuzzy("Fate"), VndbFlags.FullVisualNovel);
                        Console.WriteLine("COUT\t\t " + test.Result.Count);
                        for(int i = 0; i < test.Result.Count; i++)
                        {
                            Console.WriteLine("COUT\t\t " + test.Result.Items[i].Name);
                                  Console.WriteLine("COUT\t\t " + test.Result.Items[i].Image);
                        }
            */


            var test2 = v.GetVisualNovelListAsync(VndbFilters.UserId.Equals(44393), VndbFlags.FullVisualNovelList);
            Console.WriteLine("COUT\t\t " + test2.Result.Count);
            for (int i = 0; i < test2.Result.Count; i++)
            {
                var test3 = v.GetVisualNovelAsync(VndbFilters.Id.Equals(test2.Result.Items[i].VisualNovelId), VndbFlags.FullVisualNovel);
                for (int j = 0; j < test3.Result.Count; j++)
                {
                    Console.WriteLine("COUT\t\t " + test3.Result.Items[j].Name);
                    Console.WriteLine("COUT\t\t " + test3.Result.Items[j].OriginalName);
                    Console.WriteLine("COUT\t\t " + test3.Result.Items[j].Image);
                }
                //   Console.WriteLine("COUT\t\t " + test2.Result.Items[i].VisualNovelId);
            }
        }
    }
}
