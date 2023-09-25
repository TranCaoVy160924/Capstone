using System;
namespace Data.Enums;

public enum RoleType
{
    Admin = 1,
    Member = 2,
}
public enum NotificationSortCriteria
{
    DateCreated
}
public enum UserSortCriteria
{
    Email
}

public enum PostSortCriteria
{
    DateCreated
}

public enum CategorySortCriteria
{
    DateCreated
}

public enum TradingSortCriteria
{
    DateCreated
}

public enum ReportSortCriteria
{
    DateCreated
}

public enum TradingUserChatSortCriteria
{
    DateCreated
}

public enum MessageSortCriteria
{
    DateCreated
}

public enum TradingStatus
{
    Pending = 0,
    Processing = 1,
    Accept = 2,
    Deny = 3,
    Finish = 4
}

public enum PostType
{
    Trade = 0,
    Sell = 1,
    Free = 2
}

public enum RequestStatus
{
    Pending = 0,
    Processing = 1,
    Accept = 2,
    Deny = 3,
    Finish = 4
}

public enum ReportStatus
{
    Pending = 0,
    Processing = 1,
    Accept = 2,
    Deny = 3,
    Finish = 4
}

public enum MessageStatus
{
    Text = 0,
    Image = 1,
}

