# Halter
Uma api de organização de fichas de academia


## Registros de performance

### Exercise Entry

A `ExerciseEntry` armazena a série principal de um exercício feito <br>

A série principal é:
- **Fixed / Range / Pyramid** -> a série de maior carga
- **DropSet / UpSet** -> A sub-série de maior carga, primeira e ultima respectivamente
- **UntilFail** -> A série com maior carga. Se todas as séries tiverem as mesmas cargas, então será a série com mais repetições
- **Time** -> A série com  maior tempo feito.