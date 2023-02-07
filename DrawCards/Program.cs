namespace MyProgramm
{
    class Programm
    {
        static void Main()
        {
            Casino casino = new Casino();
            casino.ShowGameMenu();
        }
    }

    class Casino
    {


        private Dealer _dealer = new Dealer();
        private Player _player = new Player("Игрок");

        public void ShowGameMenu()
        {
            const string MenuDrawCard = "1";
            const string MenuNewGame = "2";
            const string MenuExit = "0";
            string userInput;
            bool isExit = false;
            int cardDeckPositionY = 0;
            int cardTrayPositionY = 1;
            int dealerMessagePositionY = 3;
            int scorePositionY = 5;
            int gameMenuPositionY = 7;

            _player.ClearHand();
            _dealer.BuildDeck();

            while (isExit == false)
            {
                Console.Clear();
                Console.SetCursorPosition(0, cardDeckPositionY);
                _dealer.CountDeck();
                Console.SetCursorPosition(0, cardTrayPositionY);
                _player.ShowHand();
                Console.SetCursorPosition(0, dealerMessagePositionY);
                _dealer.DecideWin(_player.CountScore());
                Console.SetCursorPosition(0, scorePositionY);
                Console.Write(_player.Name);
                _player.DisplayScore(_player.CountScore());
                Console.SetCursorPosition(0, gameMenuPositionY);
                Console.WriteLine(MenuDrawCard + " - Взять карту");
                Console.WriteLine(MenuNewGame + " - Заново");
                Console.WriteLine(MenuExit + " - Выход");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case MenuDrawCard:
                        TakeCard();
                        break;

                    case MenuNewGame:
                        ShowGameMenu();
                        break;

                    case MenuExit:
                        isExit = true;
                        break;
                }
            }
        }

        private void TakeCard()
        {
            Card card = null;

            if (_dealer.TryGiveCard(out card))
            {
                _player.AddCard(card);
            }
        }
    }

    class Dealer
    {
        private List<Card> _deck = new List<Card>();

        public void BuildDeck()
        {
            List<Card> cards = new List<Card>();
            List<int> suits = new List<int>(new int[] { 03, 04, 05, 06 });
            List<string> names = new List<string>(new string[] { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" });
            List<int> ranks = new List<int>(new int[] { 6, 7, 8, 9, 10, 8, 9, 10, 11 });
            Random random = new Random();

            cards.Clear();
            _deck.Clear();

            for (int i = 0; i < suits.Count; i++)
            {
                for (int j = 0; j < names.Count; j++)
                {
                    cards.Add(new Card(suits[i], names[j], ranks[j]));
                }
            }

            int cardsQuantity = cards.Count;

            for (int i = 0; i < cardsQuantity; i++)
            {
                _deck.Add(cards[random.Next(0, cards.Count)]);
                cards.Remove(_deck[i]);
            }
        }

        public bool TryGiveCard(out Card card)
        {
            card = null;

            if (_deck.Count <= 0)
            {
                return false;
            }
            else
            {
                card = _deck[0];
                _deck.Remove(card);
                return true;
            }
        }

        public void CountDeck()
        {
            Console.WriteLine("В колоде: " + _deck.Count + " карт.");
        }

        public bool DecideWin(int score)
        {
            int scoreToWin = 21;

            if (score > scoreToWin)
            {

                if (_deck.Count <= 0)
                {
                    Console.Write("Колода пуста");
                    return true;
                }
                else
                {
                    Console.Write("Перебор! (Вы проиграли, но можно тянуть карты дальше)");
                    return true;
                }
            }
            else if (score == scoreToWin)
            {
                Console.Write("Очко! Вы Выиграли!!! (можно тянуть карты дальше, просто чтобы посмотреть =) )");
                return false;
            }
            else
            {
                Console.Write("Играем дальше");
                return false;
            }
        }
    }

    class Card
    {
        public int Suit { get; private set; }
        public string Name { get; private set; }
        public int Rank { get; private set; }

        public Card(int suit, string name, int rank)
        {
            Suit = suit;
            Name = name;
            Rank = rank;
        }
    }

    class Player
    {
        private List<Card> _cards = new List<Card>();
        public string Name { get; private set; }

        public Player(string name)
        {
            Name = name;
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public int CountScore()
        {
            int _score = 0;

            foreach (Card card in _cards)
            {
                _score += card.Rank;
            }

            return _score;
        }

        public void DisplayScore(int score)
        {

            Console.Write($", очки: {CountScore()}");
        }

        public void ShowHand()
        {
            if (_cards.Count > 0)
            {
                foreach (Card card in _cards)
                {
                    ShowCard(card);
                }
            }
            else
            {
                Console.WriteLine("В руке нет карт");
            }
        }

        public void ClearHand()
        {
            _cards.Clear();
        }

        private void ShowCard(Card card)
        {
            Console.Write($"[{(char)card.Suit} {card.Name}]");
        }
    }
}