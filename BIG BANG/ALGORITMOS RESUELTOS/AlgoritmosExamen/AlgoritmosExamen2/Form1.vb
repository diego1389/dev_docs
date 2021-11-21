Imports System.Text.RegularExpressions

Public Class Form1
    Private Sub Button1_Click(senderecha As Object, e As EventArgs) Handles Button1.Click
        Dim algoritmoRectangulo As New RectanguloTraslapadoOeracion

        Dim recSup1 As New Punto
        recSup1.x = 1
        recSup1.y = 3
        Dim recInf1 As New Punto
        recInf1.x = 3
        recInf1.y = 3

        Dim recSup2 As New Punto
        recSup2.x = 2
        recSup2.y = 4
        Dim recInf2 As New Punto
        recInf2.x = 4
        recInf2.y = 2
        Dim resultado = algoritmoRectangulo.rectanguloTraslapa(recSup1, recInf1, recSup2, recInf2)
        If resultado = True Then
            MsgBox("Los rectangulos se traslapan")
        Else
            MsgBox("Los rectangulos no se traslapan")
        End If



    End Sub

    Private Sub Button2_Click(senderecha As Object, e As EventArgs) Handles Button2.Click
        Dim algoritmosCirculoTraslapado As New CirculosSeTraslapan
        Dim Distancia = algoritmosCirculoTraslapado.calcularDistancia(1, 2, 3, 2, 1, 1)
        MsgBox(Distancia)
        Dim valor = algoritmosCirculoTraslapado.circulosTraslapados(1, 2, 3, 2, 1, 1)
        If valor = True Then
            MsgBox("Los circulos se traslapan")
        Else
            MsgBox("Los circulos no se traslapan")
        End If
    End Sub

    Private Sub Button3_Click(senderecha As Object, e As EventArgs) Handles Button3.Click
        Dim vector(4) As Rango
        Dim vector2(3) As Rango
        Dim vector3(4) As Rango

        Dim obj0 As New Rango
        obj0.valor = 4
        obj0.vectorNombre = "vector"
        vector(0) = obj0

        Dim obj1 As New Rango


        obj1.valor = 15
        obj1.vectorNombre = "vector"

        vector(1) = obj1

        Dim obj2 As New Rango

        obj2.valor = 17
        obj2.vectorNombre = "vector"

        vector(2) = obj2

        Dim obj3 As New Rango

        obj3.valor = 24
        obj3.vectorNombre = "vector"
        vector(3) = obj3

        Dim obj4 As New Rango

        obj4.valor = 26
        obj4.vectorNombre = "vector"
        vector(4) = obj4
        '------------------------------



        Dim obj5 As New Rango
        obj5.valor = 0
        obj5.vectorNombre = "vector2"

        vector2(0) = obj5

        Dim obj6 As New Rango
        obj6.valor = 8
        obj6.vectorNombre = "vector2"
        vector2(1) = obj6

        Dim obj7 As New Rango
        obj7.valor = 10
        obj7.vectorNombre = "vector2"
        vector2(2) = obj7

        Dim obj8 As New Rango
        obj8.valor = 20
        obj8.vectorNombre = "vector2"
        vector2(3) = obj8
        '---------------------------
        Dim obj9 As New Rango

        obj9.valor = 5
        obj9.vectorNombre = "vector3"
        vector3(0) = obj9

        Dim obj10 As New Rango
        obj10.valor = 9
        obj10.vectorNombre = "vector3"
        vector3(1) = obj10

        Dim obj11 As New Rango
        obj11.valor = 18
        obj11.vectorNombre = "vector3"
        vector3(2) = obj11

        Dim obj12 As New Rango
        obj12.valor = 22
        obj12.vectorNombre = "vector3"
        vector3(3) = obj12

        Dim obj13 As New Rango
        obj13.valor = 30
        obj13.vectorNombre = "vector3"
        vector3(4) = obj13





        Dim rango As New RangoEntreVectores

        Dim rangoResultado = rango.rangoMenor(vector, vector2, vector3)
        MsgBox(rangoResultado)







    End Sub

    Private Sub Button4_Click(senderecha As Object, e As EventArgs) Handles Button4.Click
        Dim algoritmos As New SecuenciaCollage
        algoritmos.secuencia()
    End Sub

    Private Sub Button5_Click(senderecha As Object, e As EventArgs) Handles Button5.Click
        Dim algoritmos As New SecuenciaCollage
        algoritmos.secuenciaN(999999)
    End Sub

    Private Sub Button6_Click(senderecha As Object, e As EventArgs) Handles Button6.Click
        Dim algoritmos As New AcomodarNegativosPositivos
        Dim vector(8) As Integer
        algoritmos.acomodarVector(vector)
    End Sub

    Private Sub Button7_Click(senderecha As Object, e As EventArgs) Handles Button7.Click
        Dim algoritmos As New AcomodarNegativosPositivos
        Dim vector(8) As Integer
        vector(0) = -1
        vector(1) = 5
        vector(2) = -3
        vector(3) = 8
        vector(4) = 7
        vector(5) = 2
        vector(6) = -8
        vector(7) = 11
        vector(8) = 14
        algoritmos.acomodarVector(vector)
    End Sub

    Private Sub Button8_Click(senderecha As Object, e As EventArgs) Handles Button8.Click
        Dim algoritmos As New ListaPerfectosOperacion
        algoritmos.listaperfecto(100)
    End Sub

    Private Sub Button9_Click(senderecha As Object, e As EventArgs) Handles Button9.Click
        Dim algoritmos As New Anagrama
        If algoritmos.anagrama("romaa", "aamor") = True Then
            MsgBox("Son anagramas")
        Else
            MsgBox("No son anagramas")
        End If
    End Sub

    Private Sub Button10_Click(senderecha As Object, e As EventArgs) Handles Button10.Click
        Console.WriteLine("Decorador pattern\n")
        Dim component As IComponent = New DecoradorConcreto(New Component)
        cliente.Display("Componente basico", component)

    End Sub

    Private Sub Button11_Click(senderecha As Object, e As EventArgs) Handles Button11.Click

        Dim listaR As New List(Of String)
        Dim palabra As String = "abc"
        Dim tamanoPalabra As Integer
        Dim vectorPalabras() As String
        tamanoPalabra = palabra.Length

        ReDim vectorPalabras(tamanoPalabra)
        For i = 1 To Len(palabra)
            vectorPalabras(i) = Mid(palabra, i, 1)
        Next

        Dim algoritmos As New Permutacion
        algoritmos.Permurtaciones(vectorPalabras, tamanoPalabra, tamanoPalabra, listaR)
        For i = 0 To listaR.Count - 1
            Console.WriteLine(listaR(i))
        Next

    End Sub

    Private Sub Button12_Click(senderecha As Object, e As EventArgs) Handles Button12.Click
        Dim puente As New Puente
        Dim resultado = puente.cadenaColgante("*==+===+==*")
        If resultado = True Then
            MsgBox("El puente es valido")
        Else
            MsgBox("El puente es inválido")
        End If
    End Sub

    Private Sub Button13_Click(senderecha As Object, e As EventArgs) Handles Button13.Click
        Dim algoritmos As New Matriz90Grados
        Dim matriz(0, 0) As Integer
        Dim contador = 1
        For i = 0 To matriz.GetUpperBound(0)
            For j = 0 To matriz.GetUpperBound(1)
                matriz(i, j) = contador
                contador = contador + 1
            Next
        Next
        Dim matrizTemp(matriz.GetUpperBound(0), matriz.GetUpperBound(1)) As Integer
        algoritmos.matriz90Grados(matriz, matrizTemp)
    End Sub

    Private Sub Button14_Click(senderecha As Object, e As EventArgs) Handles Button14.Click
        Dim cadena As String = "*+121*21"
        Dim temp As String = String.Join("", cadena.Distinct.ToArray)
        MsgBox(temp)
        cadena = Regex.Replace(cadena, "[^*+]", "")
        MsgBox(cadena)
    End Sub

    Private Sub Button15_Click(senderecha As Object, e As EventArgs) Handles Button15.Click
        Dim cadena As String
        cadena = "aabcdefGHIJKLMNopqrstuvwxyzñ,,"
        Dim algoritmos As New Pangrama
        algoritmos.pangrama(cadena)

    End Sub

    Private Sub Button16_Click(senderecha As Object, e As EventArgs)

    End Sub

    Private Sub Button16_Click_1(senderecha As Object, e As EventArgs) Handles Button16.Click
        Dim algoritmos As New Adn
        'algoritmos.cadenaAdn("ctgactga", "actgagc")
        'algoritmos.cadenaAdn("cgtaattgcgat", "cgtacagtagc")
        algoritmos.cadenaAdn("ctgggccttgaggaaaactg", "gtaccagtactgatagt")

    End Sub

    Private Sub Button17_Click(senderecha As Object, e As EventArgs) Handles Button17.Click
        Dim algoritmos As New PalindromoPrimo
        algoritmos.menorPalindromoPrimo(456789)
    End Sub
    Class a
        Overridable Sub a()
            Console.WriteLine("a")
        End Sub
    End Class
    Class b
        Inherits a
        Overrides Sub a()
            Console.WriteLine("b")
        End Sub
    End Class
    Class c
        Inherits b
        Public Overrides Sub a()
            Console.WriteLine("c")
        End Sub
    End Class

    Private Sub Button18_Click(senderecha As Object, e As EventArgs) Handles Button18.Click
        Dim c As New c
        Dim a As a
        Dim b As b
        b = c
        a = c

        b.a()
        a.a()

    End Sub

    Private Sub Button19_Click(senderecha As Object, e As EventArgs) Handles Button19.Click
        Dim algoritmos As New Avellanas
        Dim resultado As Integer = algoritmos.chicka(2135)
        MsgBox(resultado)
    End Sub

    Private Sub Button20_Click(senderecha As Object, e As EventArgs) Handles Button20.Click
        Dim algoritmos As New cocienteResiduo
        algoritmos.cocienteResiduo(1748, 34)
    End Sub

    Private Sub Button21_Click(senderecha As Object, e As EventArgs) Handles Button21.Click
        Dim algoritmos As New RutasDiarias
        'algoritmos.rutasAlerta("ABDAC", "BD")
        'algoritmos.rutasAlerta("ABDAC", "BDX")
        algoritmos.rutasAlerta("ABDACB", "ADACX")

    End Sub

    Private Sub Button22_Click(senderecha As Object, e As EventArgs) Handles Button22.Click
        Dim algoritmos As New EncontrarPatron
        algoritmos.encuentraPatron("aalgoasisdealgoritmos", "algo")
    End Sub

    Private Sub Button23_Click(senderecha As Object, e As EventArgs) Handles Button23.Click
        Dim algoritmos As New CaracteresRepetidos
        algoritmos.eliminaReptidos("aaabcbbdfdderechaa")
    End Sub

    Private Sub Button24_Click(senderecha As Object, e As EventArgs) Handles Button24.Click

        'Dim vector(9) As Integer
        'vector(0) = 2
        'vector(1) = 1
        'vector(2) = 1
        'vector(3) = 4
        'vector(4) = 0
        'vector(5) = 1
        'vector(6) = 3
        'vector(7) = 1
        'vector(8) = 1
        'vector(9) = 1
        'Dim algoritmos As New Pastelero
        'algoritmos.pastelero(vector)

        'Dim vector(9) As Integer
        'vector(0) = 2
        'vector(1) = 2
        'vector(2) = 1
        'vector(3) = 2
        'vector(4) = 1
        'vector(5) = 1
        'vector(6) = 3
        'vector(7) = 1
        'vector(8) = 1
        'vector(9) = 1
        'Dim algoritmos As New Pastelero
        'algoritmos.pastelero(vector)

        Dim vector(9) As Integer
        vector(0) = 10
        vector(1) = 9
        vector(2) = 9
        vector(3) = 9
        vector(4) = 9
        vector(5) = 9
        vector(6) = 9
        vector(7) = 9
        vector(8) = 9
        vector(9) = 9
        Dim algoritmos As New Pastelero
        algoritmos.pastelero(vector)

    End Sub

    Private Sub Button25_Click(senderecha As Object, e As EventArgs) Handles Button25.Click

    End Sub

    Private Sub Button26_Click(senderecha As Object, e As EventArgs) Handles Button26.Click
        Dim raiz As New Nodo
        raiz.valor = 15
        Dim nodo1 As New Nodo
        nodo1.valor = 9
        raiz.izquierda = nodo1
        Dim nodo2 As New Nodo
        nodo2.valor = 20
        raiz.derecha = nodo2

        Dim nodo3 As New Nodo
        nodo3.valor = 6
        nodo1.izquierda = nodo3
        Dim nodo4 As New Nodo
        nodo4.valor = 14
        nodo1.derecha = nodo4
        Dim nodo5 As New Nodo
        nodo5.valor = 17
        nodo2.izquierda = nodo5
        Dim nodo6 As New Nodo
        nodo6.valor = 64
        nodo2.derecha = nodo6

        Dim algoritmos As New Obtener_hijos
        Dim resultado As String = algoritmos.obtenerHojas(raiz, "")
        MsgBox(resultado)

    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        Dim raiz As New NodoGrafo
        raiz.valor = 1

        Dim nodo2 As New NodoGrafo
        nodo2.valor = 2

        Dim nodo3 As New NodoGrafo
        nodo3.valor = 3

        Dim nodo4 As New NodoGrafo
        nodo4.valor = 4

        Dim nodo5 As New NodoGrafo
        nodo5.valor = 5

        Dim nodo6 As New NodoGrafo
        nodo6.valor = 6

        Dim nodo7 As New NodoGrafo
        nodo7.valor = 7

        raiz.listaNodos.Add(nodo2)
        raiz.listaNodos.Add(nodo3)
        raiz.listaNodos.Add(nodo4)
        'raiz.listaNodos.Add(nodo4)

        nodo2.listaNodos.Add(nodo5)
        nodo5.listaNodos.Add(nodo7)

        nodo4.listaNodos.Add(nodo6)

        Dim algoritmos As New Grafo
        Dim lista As New List(Of Integer)
        Dim listaFinal As New List(Of Integer)
        listaFinal = algoritmos.recorridoProfunidad(raiz, lista)
        For Each nodo In listaFinal
            Console.WriteLine(nodo)
        Next


    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        Dim j As New J
        Dim m As M = j
        m.f()
    End Sub
    Class M
        Public Overridable Sub f()
            Console.WriteLine("M")
        End Sub
    End Class
    Class O
        Inherits M
        Public Overrides Sub f()
            Console.WriteLine("o")
        End Sub
    End Class
    Class N
        Inherits O
        Public Overridable Sub f()
            Console.WriteLine("n")
        End Sub
    End Class
    Class J
        Inherits N
        Public Overrides Sub f()
            Console.WriteLine("j")
        End Sub
    End Class
    Class Z
        Inherits M
        Public Overrides Sub f()
            Console.WriteLine("z")
        End Sub
    End Class

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        Dim raiz As New NodoGrafo
        raiz.valor = 1

        Dim nodo2 As New NodoGrafo
        nodo2.valor = 2

        Dim nodo3 As New NodoGrafo
        nodo3.valor = 3

        Dim nodo4 As New NodoGrafo
        nodo4.valor = 4

        Dim nodo5 As New NodoGrafo
        nodo5.valor = 5

        Dim nodo6 As New NodoGrafo
        nodo6.valor = 6

        Dim nodo7 As New NodoGrafo
        nodo7.valor = 7

        raiz.listaNodos.Add(nodo2)
        raiz.listaNodos.Add(nodo3)
        raiz.listaNodos.Add(nodo4)
        'raiz.listaNodos.Add(nodo4)

        nodo2.listaNodos.Add(nodo5)
        nodo5.listaNodos.Add(nodo7)

        nodo4.listaNodos.Add(nodo6)

        Dim algoritmos As New Grafo
        Dim lista As New List(Of Integer)
        Dim listaFinal As New List(Of Integer)
        algoritmos.recorridoAnchura(raiz, lista)
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        Dim diccionario As New List(Of String)

        diccionario.Add("adios")
        diccionario.Add("al")
        diccionario.Add("Con")
        diccionario.Add("simpe")
        diccionario.Add("movil")
        diccionario.Add("digale")
        diccionario.Add("efectivo")

        Dim divideCadena As New DivideCadenaConEspacios
        divideCadena.dividirCadena("Consimpemovildigaleadiosalefectivo", diccionario)
    End Sub

    Private Sub Button31_Click(sender As Object, e As EventArgs) Handles Button31.Click
        Dim algoritmos As New CuentaVocalesConsonantes
        algoritmos.cuentaVocalesConsonan("ABC 234AA")
    End Sub

    Private Sub Button32_Click(sender As Object, e As EventArgs) Handles Button32.Click

    End Sub

    Private Sub Button33_Click(sender As Object, e As EventArgs) Handles Button33.Click
        Dim algoritmos As New cadenaDiferentes
        Dim resultado As Integer = algoritmos.mayorCadena("baeefeeebeab")
        MsgBox(resultado)
    End Sub

    Private Sub Button34_Click(sender As Object, e As EventArgs) Handles Button34.Click
        'Dim resultado As Integer = Rnd() * 10
        'MsgBox(resultado)
        Dim algoritmos As New BuscaMinas
        Dim matriz(2, 2) As String
        matriz(0, 0) = "*"
        matriz(0, 1) = "*"
        matriz(0, 2) = "*"
        matriz(2, 0) = "*"
        matriz(2, 2) = "*"

        algoritmos.buscaMinas(matriz)

    End Sub

    Private Sub Button35_Click(sender As Object, e As EventArgs) Handles Button35.Click
        Dim cadena As String = "1-2-3-3-4-5-6"
        Dim algoritmos As New NumerosRepetidos
        algoritmos.repetidosNumeros(cadena)
    End Sub

    Private Sub Button36_Click(sender As Object, e As EventArgs) Handles Button36.Click
        Dim dataAccess As New DataAcess(New SQLConex(""))
        dataAccess.EjecutarSP("spInsert")
    End Sub

    Private Sub Button37_Click(sender As Object, e As EventArgs) Handles Button37.Click
        Dim vector(4) As Rango
        Dim vector2(3) As Rango
        Dim vector3(4) As Rango

        Dim obj0 As New Rango
        obj0.valor = 4
        obj0.vectorNombre = "vector"
        vector(0) = obj0

        Dim obj1 As New Rango


        obj1.valor = 15
        obj1.vectorNombre = "vector"

        vector(1) = obj1

        Dim obj2 As New Rango

        obj2.valor = 17
        obj2.vectorNombre = "vector"

        vector(2) = obj2

        Dim obj3 As New Rango

        obj3.valor = 24
        obj3.vectorNombre = "vector"
        vector(3) = obj3

        Dim obj4 As New Rango

        obj4.valor = 26
        obj4.vectorNombre = "vector"
        vector(4) = obj4
        '------------------------------



        Dim obj5 As New Rango
        obj5.valor = 0
        obj5.vectorNombre = "vector2"

        vector2(0) = obj5

        Dim obj6 As New Rango
        obj6.valor = 8
        obj6.vectorNombre = "vector2"
        vector2(1) = obj6

        Dim obj7 As New Rango
        obj7.valor = 10
        obj7.vectorNombre = "vector2"
        vector2(2) = obj7

        Dim obj8 As New Rango
        obj8.valor = 20
        obj8.vectorNombre = "vector2"
        vector2(3) = obj8
        '---------------------------
        Dim obj9 As New Rango

        obj9.valor = 5
        obj9.vectorNombre = "vector3"
        vector3(0) = obj9

        Dim obj10 As New Rango
        obj10.valor = 9
        obj10.vectorNombre = "vector3"
        vector3(1) = obj10

        Dim obj11 As New Rango
        obj11.valor = 18
        obj11.vectorNombre = "vector3"
        vector3(2) = obj11

        Dim obj12 As New Rango
        obj12.valor = 22
        obj12.vectorNombre = "vector3"
        vector3(3) = obj12

        Dim obj13 As New Rango
        obj13.valor = 30
        obj13.vectorNombre = "vector3"
        vector3(4) = obj13

        Dim rango As New RangoMenorMejorado
        'rango.vectorMasGrande(vector, vector2, vector3)
        'rango.vectorMasGrande(vector2, vector3, vector)
        rango.vectorMasGrande(vector3, vector, vector2)


    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles Button38.Click
        Console.WriteLine("Decorador pattern\n")
        Dim component As IComponent = New DecoradorConcreto(New Component)
        cliente.Display("Componente basico", component)

        Dim dibujo As IDibujar = New DecoradorConcretoDibujoRojo(New Triangulo)
        Dim resultado = dibujo.dibujar
        MsgBox(resultado)
    End Sub

    Private Sub Button39_Click(sender As Object, e As EventArgs) Handles Button39.Click
        Dim candidato1 As New Candidato
        candidato1.nombre = "pedro"
        candidato1.puntaje = 0

        Dim candidato2 As New Candidato
        candidato2.nombre = "juan"
        candidato2.puntaje = 0

        Dim candidato3 As New Candidato
        candidato3.nombre = "Marvin"
        candidato3.puntaje = 0

        Dim candidato4 As New Candidato
        candidato4.nombre = "roberto"
        candidato4.puntaje = 0

        Dim candidato5 As New Candidato
        candidato5.nombre = "Romeo"
        candidato5.puntaje = 0



        Dim candidatos As New List(Of Candidato)

        candidatos.Add(candidato1)
        candidatos.Add(candidato2)
        candidatos.Add(candidato3)
        candidatos.Add(candidato4)
        candidatos.Add(candidato5)
        Dim listaResultado As New List(Of Candidato)

        Dim algoritmos As New Candidatos
        listaResultado = algoritmos.candidatos(candidatos)

        For Each current In listaResultado
            Console.WriteLine("El candidato:" & current.nombre & " puntaje :" & current.puntaje)
        Next



    End Sub
End Class
