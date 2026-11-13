namespace LookupsDemo.Services;

// A small DTO: the lookup service does not depend on MVC or Razor.
public record LookupItem(string Value, string Text);
