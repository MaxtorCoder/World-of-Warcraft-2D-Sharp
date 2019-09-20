namespace Framework.Network.Packet.Opcodes
{
    public enum GameOpcodes
    {
        // Client Messages
        LogonRequest                = 0x2000,
        RealmlistRequest            = 0x2001,
        CharacterListRequest        = 0x2002,
        CharacterCreate             = 0x2003,
        CharacterDelete             = 0x2004,
        WorldEnter                  = 0x2005,
        WorldRequest                = 0x2006,
        MovementUpdate              = 0x2007,
        ChatSend                    = 0x2008,
        RequestMOTD                 = 0x2009,
        WhoListRequest              = 0x2010,
        CreatureListRequest         = 0x2011,

        // Server Messages
        LogonResponse               = 0x3000,
        RealmListResponse           = 0x3001,
        CharacterListResponse       = 0x3002,
        CharacterResponse           = 0x3003,
        ConnectionAdd               = 0x3004,
        ConnectionRemove            = 0x3005,
        ConnectionMove              = 0x3006,
        CreatureListResponse        = 0x3007,
        WhoListResponse             = 0x3008,
        Disconnect                  = 0x3009,

        Unknown                     = 0xBADD,
        None                        = 0,
    }
}
