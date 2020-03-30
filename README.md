# Tests

```csharp
int Amount = 0;
foreach(var c in word)
{
   if(c == character) Amount++;
}
return Amount == AmountOfSpecificCharacter;
```


# Word being tested: ALFABETET
## Test Before  (StringContainsSameAmountOfCharacters)
 1. **247 Words (Adding "A" 2 Positions)**
 2. **89 Words (Removing "S")**
 3. **39 Words (Removing "B")**
 4. **2 Words (Adding "T" 2 Positions)**
 5. **COMPLETED (Adding "E" 2 Positions)**

## Test With Added (StringContainsSameAmountOfCharacters)
 1. **117 Words (Adding "A" 2 Positions)**
 2. **32 Words (Removing "S")**
 3. **2 Words (Adding "T" 2 Positions)**
 4. **COMPLETED (Adding "E" 2 Positions)**

 __Conclusion__
 Saved 1 try on first example while still being a relatively bad word example.
 Might save up to 3 Tries or more with different words.
 ___
