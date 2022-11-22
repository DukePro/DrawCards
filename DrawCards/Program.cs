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
        const string MenuDrawCard = "1";
        const string MenuNewGame = "2";
        const string MenuExit = "0";

        private bool _isExit = false;
        private string _userInput;
        private int _cardDeckPositionY = 0;
        private int _cardTrayPositionY = 1;
        private int _dealerMessagePositionY = 3;
        private int _scorePositionY = 5;
        private int _gameMenuPositionY = 7;
        private Dealer _dealer = new Dealer();
        private Player _player = new Player("Игрок");

        public void ShowGameMenu()
        {
            _isExit = false;
            _player.ClearHand();
            _dealer.BuildDeck();

            while (_isExit == false)
            {
                Console.Clear();
                Console.SetCursorPosition(0, _cardDeckPositionY);
                _dealer.CountDeck();
                Console.SetCursorPosition(0, _cardTrayPositionY);
                _player.ShowHand();
                Console.SetCursorPosition(0, _dealerMessagePositionY);
                _dealer.DecideWin(_player.CountScore());
                Console.SetCursorPosition(0, _scorePositionY);
                DisplayScore(_player.CountScore());
                Console.SetCursorPosition(0, _gameMenuPositionY);
                Console.WriteLine(MenuDrawCard + " - Взять карту");
                Console.WriteLine(MenuNewGame + " - Заново");
                Console.WriteLine(MenuExit + " - Выход");
                _userInput = Console.ReadLine();

                switch (_userInput)
                {
                    case MenuDrawCard:
                        _player.Add(_dealer.GiveCard());
                        break;
                    case MenuNewGame:
                        ShowGameMenu();
                        break;
                    case MenuExit:
                        _isExit = true;
                        break;
                }
            }
        }

        private void DisplayScore(int score)
        {
            Console.Write($"{_player.Name}, очки: {_player.CountScore()}");
        }
    }

    class Dealer
    {
        private List<Card> _cards = new List<Card>();
        private List<Card> _deck = new List<Card>();
        private List<int> _suits = new List<int>(new int[] { 03, 04, 05, 06 });
        private List<string> _names = new List<string>(new string[] { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" });
        private List<int> _ranks = new List<int>(new int[] { 6, 7, 8, 9, 10, 8, 9, 10, 11 });
        private Random _random = new Random();

        public void BuildDeck()
        {
            _cards.Clear();
            _deck.Clear();

            for (int i = 0; i < _suits.Count; i++)
            {
                for (int j = 0; j < _names.Count; j++)
                {
                    _cards.Add(new Card(_suits[i], _names[j], _ranks[j]));
                }
            }

            int cardsQuantity = _cards.Count;

            for (int i = 0; i < cardsQuantity; i++)
            {
                _deck.Add(_cards[_random.Next(0, _cards.Count)]);
                _cards.Remove(_deck[i]);
            }
        }

        public Card GiveCard()
        {
            Card cardToGive = null;

            if (_deck.Count <= 0)
            {
                Console.WriteLine("Колода пуста");
                return cardToGive; //Я знаю, что вернёт null и всё упадёт, задал вопрос в чате, пока не знаю как сделать тут правильно. Нет ответа, сдаю, чтобы проверить остальное.
            }
            else
            {
                cardToGive = _deck[0];
                _deck.Remove(cardToGive);
                return cardToGive;
            }
        }

        public void CountDeck()
        {
            Console.WriteLine("В колоде: " + _deck.Count + " карт.");
        }

        public bool DecideWin(int score)
        {
            if (score > 21)
            {
                Console.Write("Перебор! (Вы проиграли, но можно тянуть карты дальше)");
                return true;
            }
            else if (score == 21)
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
        private int _score;
        public string Name { get; private set; }

        public Player(string name)
        {
            Name = name;
        }

        public void Add(Card card)
        {
            _cards.Add(card);
        }

        public int CountScore()
        {
            _score = 0;

            foreach (Card card in _cards)
            {
                _score += card.Rank;
            }

            return _score;
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

        public void ShowCard(Card card)
        {
            Console.Write($"[{(char)card.Suit} {card.Name}]");
        }
    }
}