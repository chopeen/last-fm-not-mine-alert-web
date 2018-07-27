namespace last_fm_not_mine_alert_web
{
    static class Consts
    {
        internal const string ARTIST_NAME_VALIDATION_REGEX = @"^[A-Za-z0-9&.,\- \p{IsLatin-1Supplement}]+$";
        //                                                       A-Za-z                                 - letters
        //                                                             0-9                              - digits
        //                                                                &.,\-                         - selected punctuation
        //                                                                      \p{IsLatin-1Supplement} - diacritic characters and more
        // This expression cannot be generated with a function, because an attribute argument must be a constant expression,
        //   typeof expression or array creation expression of an attribute parameter type
    }
}