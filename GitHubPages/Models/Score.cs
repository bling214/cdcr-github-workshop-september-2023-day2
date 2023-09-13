namespace GitHubPages.Models
{
    public class Score
    {
        public string CategoryName { get; set; } = string.Empty;
        public int Correct { get; set; }
        public int Total { get; set; }

        public int Percent
        {
            get
            {
                if (this.Total == 0)
                {
                    return 0;
                }
                else
                {
                    return (int)((double)this.Correct / this.Total * 100);
                }
            }
        }
    }
}
