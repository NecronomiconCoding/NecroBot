using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using System;

namespace PoGo.NecroBot.Logic
{
    public class KillSwitch
    {
        private DateTime CatchErrorHours;
        private int CountCatchError;
        private DateTime CatchEscapeHours;
        private int CountCatchEscape;
        private DateTime CatchFleeHours;
        private int CountCatchFlee;
        private DateTime CatchMissedHours;
        private int CountCatchMissed;
        private DateTime CatchSuccessHours;
        private int CountCatchSuccess;
                
        public void CatchError(ISession session)
        {
            CountCatchError++;

            if (CatchErrorHours == DateTime.MinValue)
            {
                CatchErrorHours = DateTime.Now;
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchErrorState,
                        CountCatchError, session.LogicSettings.CatchErrorPerHours),
                });
                return;
            }
            
            var requestHours = (DateTime.Now - CatchErrorHours).TotalHours;
            if (requestHours <= 1 && CountCatchError >= session.LogicSettings.CatchErrorPerHours)
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchErrorExceeded,
                        CountCatchError, session.LogicSettings.CatchErrorPerHours),
                    RequireStop = true
                });

                Console.ReadLine();
                Environment.Exit(0);
            }
            else if (requestHours >= 1 && CountCatchError < session.LogicSettings.CatchErrorPerHours)
            {
                CountCatchError = 1;
                CatchErrorHours = DateTime.Now;

                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchErrorReset,
                        CountCatchError, session.LogicSettings.CatchErrorPerHours),
                });
            }
            else
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchErrorState,
                        CountCatchError, session.LogicSettings.CatchErrorPerHours),
                });
            }
        }

        public void CatchEscape(ISession session)
        {
            CountCatchEscape++;

            if (CatchEscapeHours == DateTime.MinValue)
            {
                CatchEscapeHours = DateTime.Now;
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchEscapeState,
                        CountCatchEscape, session.LogicSettings.CatchEscapePerHours),
                });
                return;
            }
            
            var requestHours = (DateTime.Now - CatchEscapeHours).TotalHours;
            if (requestHours <= 1 && CountCatchEscape >= session.LogicSettings.CatchEscapePerHours)
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchEscapeExceeded,
                        CountCatchEscape, session.LogicSettings.CatchEscapePerHours),
                    RequireStop = true
                });

                Console.ReadLine();
                Environment.Exit(0);
            }
            else if (requestHours >= 1 && CountCatchEscape < session.LogicSettings.CatchEscapePerHours)
            {
                CountCatchEscape = 1;
                CatchEscapeHours = DateTime.Now;

                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchEscapeReset,
                        CountCatchError, session.LogicSettings.CatchEscapePerHours),
                });
            }
            else
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchEscapeState,
                        CountCatchEscape, session.LogicSettings.CatchEscapePerHours),
                });
            }
        }

        public void CatchFlee(ISession session)
        {
            CountCatchFlee++;

            if (CatchFleeHours == DateTime.MinValue)
            {
                CatchFleeHours = DateTime.Now;
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchFleeState,
                        CountCatchFlee, session.LogicSettings.CatchFleePerHours),
                });
                return;
            }
                        
            var requestHours = (DateTime.Now - CatchFleeHours).TotalHours;
            if (requestHours <= 1 && CountCatchFlee >= session.LogicSettings.CatchFleePerHours)
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchFleeExceeded,
                        CountCatchFlee, session.LogicSettings.CatchFleePerHours),
                    RequireStop = true
                });

                Console.ReadLine();
                Environment.Exit(0);
            }
            else if (requestHours >= 1 && CountCatchFlee < session.LogicSettings.CatchFleePerHours)
            {
                CountCatchFlee = 1;
                CatchFleeHours = DateTime.Now;

                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchFleeReset,
                        CountCatchFlee, session.LogicSettings.CatchFleePerHours),
                });
            }
            else
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchFleeState,
                        CountCatchFlee, session.LogicSettings.CatchFleePerHours),
                });
            }
        }

        public void CatchMissed(ISession session)
        {
            CountCatchMissed++;

            if (CatchMissedHours == DateTime.MinValue)
            {
                CatchMissedHours = DateTime.Now;
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchMissedState,
                        CountCatchMissed, session.LogicSettings.CatchMissedPerHours),
                });
                return;
            }
            
            var requestHours = (DateTime.Now - CatchMissedHours).TotalHours;
            if (requestHours <= 1 && CountCatchMissed >= session.LogicSettings.CatchMissedPerHours)
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchMissedExceeded,
                        CountCatchMissed, session.LogicSettings.CatchMissedPerHours),
                    RequireStop = true
                });

                Console.ReadLine();
                Environment.Exit(0);
            }
            else if (requestHours >= 1 && CountCatchMissed < session.LogicSettings.CatchMissedPerHours)
            {
                CountCatchMissed = 1;
                CatchMissedHours = DateTime.Now;

                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchMissedReset,
                        CountCatchMissed, session.LogicSettings.CatchMissedPerHours),
                });
            }
            else
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchMissedState,
                        CountCatchMissed, session.LogicSettings.CatchMissedPerHours),
                });
            }
        }

        public void CatchSuccess(ISession session)
        {
            CountCatchSuccess++;

            if (CatchSuccessHours == DateTime.MinValue)
            {
                CatchSuccessHours = DateTime.Now;
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchSuccessState,
                        CountCatchSuccess, session.LogicSettings.CatchSuccessPerHours),
                });
                return;
            }
            
            var requestHours = (DateTime.Now - CatchSuccessHours).TotalHours;
            if (requestHours <= 1 && CountCatchSuccess >= session.LogicSettings.CatchSuccessPerHours)
            {
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchSuccessExceeded,
                        CountCatchSuccess, session.LogicSettings.CatchSuccessPerHours),
                    RequireStop = true
                });

                Console.ReadLine();
                Environment.Exit(0);
            }
            else if (requestHours >= 1 && CountCatchSuccess < session.LogicSettings.CatchSuccessPerHours)
            {
                CountCatchSuccess = 1;
                CatchSuccessHours = DateTime.Now;

                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchSuccessReset,
                        CountCatchSuccess, session.LogicSettings.CatchSuccessPerHours),
                });
            }
            else
            {
            
                session.EventDispatcher.Send(new KillSwitchEvent
                {
                    Message = session.Translation.GetTranslation(
                        TranslationString.CatchSuccessState,
                        CountCatchSuccess, session.LogicSettings.CatchSuccessPerHours),
                });
            }
        }
    }
}
