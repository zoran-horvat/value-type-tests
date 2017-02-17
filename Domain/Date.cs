using System;

namespace Domain
{
    public sealed class Date : IEquatable<Date>
    {
        public int Year => this.FullTime.Year;
        public int Month => this.FullTime.Month;
        public int Day => this.FullTime.Day;

        private DateTime FullTime { get; }

        public Date(int year, int month, int day)
        {
            this.FullTime = new DateTime(year, month, day);
        }

        public Date(DateTime date) 
            : this(date.Year, date.Month, date.Day)
        {
        }

        public Date AddDays(int days) =>
            new Date(this.FullTime.AddDays(days));

        public bool Equals(Date other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FullTime.Equals(other.FullTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Date) obj);
        }

        public override int GetHashCode()
        {
            return FullTime.GetHashCode();
        }

        public static bool operator ==(Date a, Date b)
        {
            if (ReferenceEquals(a, null))
                return ReferenceEquals(b, null);
            return a.Equals(b);
        }

        public static bool operator !=(Date a, Date b) => !(a == b);
    }
}
