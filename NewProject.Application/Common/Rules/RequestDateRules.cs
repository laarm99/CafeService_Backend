namespace NewProject.Application.Common.Rules
{
    // Reglas de fechas de las solicitudes. Deben coincidir con utils/date-rules.js del frontend.
    public static class RequestDateRules
    {
        // La semana de nomina corta los lunes: la fecha mas antigua permitida es el lunes de la semana actual.
        public static DateTime EarliestAllowedDate(DateTime today)
        {
            var offset = ((int)today.DayOfWeek + 6) % 7;
            return today.Date.AddDays(-offset);
        }

        public static string? ValidateStart(DateTime date, string label, DateTime? today = null)
        {
            var now = (today ?? DateTime.Today).Date;
            var earliest = EarliestAllowedDate(now);
            return date.Date < earliest
                ? $"{label} no puede ser anterior al {earliest:dd/MM/yyyy} (corte de nómina)."
                : null;
        }

        public static string? ValidateRange(DateTime start, DateTime end, DateTime returnDate, DateTime? today = null)
        {
            var error = ValidateStart(start, "La fecha de inicio", today);
            if (error != null)
            {
                return error;
            }

            if (end.Date < start.Date)
            {
                return "La fecha final no puede ser anterior a la fecha de inicio.";
            }

            if (returnDate.Date <= end.Date)
            {
                return "La fecha de regreso debe ser posterior a la fecha final.";
            }

            return null;
        }

        public static string? ValidateAdjustmentDate(DateTime date, DateTime? today = null)
        {
            var now = (today ?? DateTime.Today).Date;
            if (date.Date > now)
            {
                return "La fecha del ajuste no puede ser futura.";
            }

            return ValidateStart(date, "La fecha del ajuste", now);
        }
    }
}
