namespace NyaaApi_DotNet.API
{
    class SukeibeApi
    {
        public static readonly string SUKEIBE = "https://nyaa-api-adv.herokuapp.com/sukeibe";
        public static readonly string SLASH = "/";
        public static readonly string AND = "&";
        public static readonly string QUEST = "?";
    }
    class SukeibeApiParameter
    {
        public static readonly string TITLE = "q=";
        public static readonly string CATE = "category=";
        public static readonly string SUBCATE = "subcategory=";
        public static readonly string PAGE = "page=";
    }
}
/*# categories = {
#     "1": {
#         "name": "Art",
#         "subcats": {
#             "1": "Anime",
#             "2": "Doujinshi",
#             "3": "Games",
#             "4": "Manga",
#             "5": "Pictures",
#         }
#     },
#     "2": {
#         "name": "Real Life",
#         "subcats": {
#             "1": "Photobooks & Pictures",
#             "2": "Videos"
#         }
#     }
# }*/