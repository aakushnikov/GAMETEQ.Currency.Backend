using Microsoft.AspNetCore.Mvc;

namespace GAMETEQ.Currency.Http;

public abstract class HttpRequestBase
{
    public virtual string ToQuery(string dateTimeFormat = "yyyy-MM-dd")
    {
        var properties = GetType().GetProperties();

        var pairs = new Dictionary<string, string>();

        foreach (var info in properties)
        {
            var attributes = info.GetCustomAttributesData();
            var attributeData = attributes.FirstOrDefault(a => a.AttributeType == typeof(FromQueryAttribute));
            if (attributeData is null) continue;

            var query = attributeData
                .NamedArguments
                .FirstOrDefault(f => f.MemberName == nameof(FromQueryAttribute.Name))
                .TypedValue.Value;
                            
            var value = info.GetValue(this);
            
            if (value is null) continue;

            pairs.Add(
                Convert.ToString(query)
                    ?? throw new NotImplementedException(
                        $"{nameof(FromQueryAttribute)} in {GetType().Name} should have specified value"),
                value is DateTime dateTime
                    ? dateTime.ToString(dateTimeFormat)
                    : Convert.ToString(value) ?? string.Empty
            );
        }

        return string.Concat("?", string.Join('&', pairs.Select(kv => string.Concat(kv.Key, "=", kv.Value))));
    }
}