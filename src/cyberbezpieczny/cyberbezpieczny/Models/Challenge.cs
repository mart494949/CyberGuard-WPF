namespace cyberbezpieczny.Models
{
    // 1. Klasa Challenge (Pojedyncze pytanie z JSON)
    public class Challenge
    {
        public string QuestionText { get; set; }
        public string ImagePath { get; set; }
        public bool IsSafe { get; set; }
        public string Explanation { get; set; }
    }

    // 2. Klasa EmailTask 
    // Służy do wyświetlania maili w grze
    public class EmailTask
    {
        public string Sender { get; set; }      // Nadawca
        public string Subject { get; set; }     // Temat
        public string Content { get; set; }     // Treść
        public bool IsPhishing { get; set; }    // Czy to atak?
        public string Explanation { get; set; } // Wyjaśnienie
    }
}