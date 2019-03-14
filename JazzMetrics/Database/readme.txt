update se dela v Tools –> NuGet Package Manager –> Package Manager Console

pozor - maze puvodni soubory

nutne mit jako Default project projekt Database (primo v selectboxu v okne)

zada se (samozrejme spravny nazev serveru) ->
Scaffold-DbContext "Server=MICHAL-PC3;Database=JazzMetrics;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -Output DAO [-t nazevTabulky] [-f]
Scaffold-DbContext "Server=dbsys.cs.vsb.cz\STUDENT;Database=pri0115;user id=pri0115;password=wu3d623pK7;" Microsoft.EntityFrameworkCore.SqlServer -Output DAO [-t nazevTabulky] [-f]

-f = with force

vytvori to i tridu xxContext a tu je potreba smazat

nutne vzdy obnovit CELY DB model!