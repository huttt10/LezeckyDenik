using LezeckyDenik.Models.StaticData;

namespace LezeckyDenik.Utility
{
    public static class ConverterDifficulty
    {
        public static int GetIntFromDifficultyString(string difficulty)
        {
            List<string> difficultyList = ListOfUIAADifficulty.GetList();
            int difficultyCoverted = difficultyList.FindIndex(x => x == difficulty);
            difficultyCoverted = difficultyCoverted + 1;
            return difficultyCoverted;
        }

        public static string GetStringFromDifficultyInt(int numberConvertedToDifficulty)
        {
            List<string> difficultyList = ListOfUIAADifficulty.GetList();
            string difficultyString = difficultyList[numberConvertedToDifficulty - 1];
            return difficultyString;
        }
    }
}
