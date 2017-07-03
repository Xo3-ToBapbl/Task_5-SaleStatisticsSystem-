namespace StatisticSystem.PL.Utills
{
    public static class MessagesPL
    {
        public static string FilterMessage
        {
            get
            {
                return "Select a filter to display data";
            }
        }

        public static string EmptyFieldMessage
        {
            get
            {
                return "Please, enter data";
            }
        }

        public static string EmptyDataMessage
        {
            get
            {
                return "There is no data for your request";
            }
        }

        public static string InvalidDateMessage
        {
            get
            {
                return "Please, enter valid date";
            }
        }

        public static string ErrorDeleteUserMessage
        {
            get
            {
                return "You can not delete yourself, administrators or a users with an undefined role. Please, call servicemanager";
            }
        }

        public static string EmptyUserDeleteMessage
        {
            get
            {
                return "There is no manager to delete";
            }
        }

        public static string EmptySaleEditMessage
        {
            get
            {
                return "There is no sale to edit";
            }
        }
    }
}