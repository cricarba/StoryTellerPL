
namespace Cricarba.StoryTellerPL.Dto
{
    internal class TweetST
    {
        public string Template { get; set; }
        public int Time { get; set; }
        public bool HasImage { get; set; }
        public string Image { get; set; }
        public bool  IsHalfTime { get { return Template.Contains("Half-time"); } }
        public bool IsEndTime { get { return Template.Contains("Full-time")|| Template.Contains("Second Half ends"); } }
    }
}
