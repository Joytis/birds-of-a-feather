namespace LWG {
public class Tuple<T1, T2> {
	public T1 Item1 { get; set; }
	public T2 Item2 { get;  set; }

	public Tuple(T1 first, T2 second) {
		Item1 = first;
		Item2 = second;
	}

	public override int GetHashCode() => Item1.GetHashCode() ^ Item2.GetHashCode(); 

    public override bool Equals(object obj) => (obj is Tuple<T1,T2> && Equals((Tuple<T1,T2>) obj));

    public bool Equals(Tuple<T1,T2> tup) => (Item1.Equals(tup.Item1) && Item2.Equals(tup.Item2));

	public override string ToString() => $"({Item1}, {Item2})";

	public Tuple<T1, T2> Copy() => new Tuple<T1, T2>(Item1, Item2);

}

public class Tuple<T1, T2, T3> {
	public T1 Item1 { get; set; }
	public T2 Item2 { get; set; }
	public T3 Item3 { get; set; }

	public Tuple(T1 first, T2 second, T3 third) {
		Item1 = first;
		Item2 = second;
		Item3 = third;
	}

	public override int GetHashCode() => Item1.GetHashCode() ^ Item2.GetHashCode() ^ Item3.GetHashCode();

    public override bool Equals(object obj) => obj is Tuple<T1,T2, T3> && Equals((Tuple<T1,T2, T3>) obj);

    public bool Equals(Tuple<T1,T2, T3> tup) => Item1.Equals(tup.Item1) && Item2.Equals(tup.Item2) && Item3.Equals(tup.Item3);

	public override string ToString() => $"({Item1}, {Item2}, {Item3})";

	public Tuple<T1, T2, T3> Copy() => new Tuple<T1, T2, T3>(Item1, Item2, Item3);

}

}