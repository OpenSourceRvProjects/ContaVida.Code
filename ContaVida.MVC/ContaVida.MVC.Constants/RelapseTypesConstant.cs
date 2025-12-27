using ContaVida.MVC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaVida.MVC.Constants
{
    public class RelapseTypesConstant
    {
        public static int NoReason = 0;
        public static int SocialEventReason = 1;
        public static int ExtremeAnxiety = 2;
        public static int SocialPushing = 3;
        public static int ExtremeStress = 4;
        public static int FamiliarConflicts = 5;
        public static int TherapyLeave = 6;
        public static int ControlLoose = 7;
        public static int FamiliarLoss = 8;
        public static int ForFun = 9;
        public static int Sickness = 10;

        public static int Others = 99;

        public static List<TextValueModel> GetRelapseReasons()
        {
            return new List<TextValueModel> {
                new TextValueModel(RelapseTypesConstant.NoReason, "-------------------------------"),
                new TextValueModel(RelapseTypesConstant.SocialEventReason, "Evento social (cumpleaños, fiesta)"),
                new TextValueModel(RelapseTypesConstant.ExtremeAnxiety, "Ansiedad extrema"),
                new TextValueModel(RelapseTypesConstant.SocialPushing, "Presión social"),
                new TextValueModel(RelapseTypesConstant.ExtremeStress, "Estrés extremo"),
                new TextValueModel(RelapseTypesConstant.FamiliarConflicts, "Conflicto familiar"),
                new TextValueModel(RelapseTypesConstant.TherapyLeave, "Abandono de terapia o tratamiento"),
                new TextValueModel(RelapseTypesConstant.ControlLoose, "Pérdida de control"),
                new TextValueModel(RelapseTypesConstant.FamiliarLoss, "Luto"),
                new TextValueModel(RelapseTypesConstant.ForFun, "Momento lúdico"),
                new TextValueModel(RelapseTypesConstant.Sickness, "Enfermedad"),
                new TextValueModel(RelapseTypesConstant.Others, "Otros motivos..."),

            };
        }
    }

}
