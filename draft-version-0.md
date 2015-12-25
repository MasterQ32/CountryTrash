# Harvest Boon

## Grundgedanke

Ein Farmspiel, in welchem der Spieler Pflanzen anbaut und Tiere züchtet.
Das Spiel ist ein Endlosspiel, welches kein weiteres Ziel verfolgt.

## Features

Der Spieler hat folgende Möglichkeiten:

- In der Welt herumlaufen mit Wegsuche
- Einfacher Karteneditor mit Tastaturbedienung
- Wechseln der aktuellen Spielkarte über betreten eines Feldes
- Aktionen mit Warteschleife
	- Umgraben von Landfeldern zu Ackerfeldern
	- Anpflanzen
	- Gießen
	- Ernten
- Sammeln von Gegenständen in einem Inventar

Zudem läuft das Spiel im Multiplayer ab, es können also auf einer Karte mehrere Spieler
die oben genannten Aktionen durchführen. Jeder Spieler besitzt dabei seine eigene Warteschlange.

## Konzeptideen

### Warteschlange und Bewegung
Das Spiel stellt einen Tiny Client dar, welcher alle Aktionen, die der Spieler ausführen will,
an einen Server sendet. Der Server führt die gesamte Spielsimulation aus und sendet Veränderungen
in der Spielwelt zurück an die Spieler.

### Wegsuche
Die Wegsuche wird über einen modifizierten Floodfill-Suchalgorithmus gelöst, welcher ideal
auf Tilemaps zu verwenden ist.

### Bestandteile Karten

Die Karten bestehen aus drei Hauptkomponenten. 

#### Tiles
Eine Tile ist ein quadratisches, grafisches Bodenelement, welches entweder ein Betreten
ermöglicht oder verhindert. Zudem können Tiles verschiedene Aktionen ausführen, wenn eines
der folgenden Events eintritt:

- Entität betritt Tile
- Entität verlässt Tile
- Spieler verwendet Tile
- Ein Tick tritt ein

#### Entitäten
Entitäten sind Objekte, welche auf der Karte stehen und keine Tiles sind. Sie können sich
bewegen und können den Weg bei der Wegsuche versperren.

Beispiele für Entitäten sind:

- Spieler
- Tiere
- Gebäude
- Türen

Entitäten können ebenfalls Events empfangen:

- Spieler klickt Entität an
- Entität kollidiert mit anderer Entität
- Entität betritt Tile
- Entität verlässt Tile
- Ein Tick tritt ein

#### Prozesse
Prozesse laufen im Hintergrund einer Karte ab und haben keine Visualisierung. Sie können
Einfluss auf den generellen Spielverlauf auf der Karte nehmen.

### Spielwelt (Ab Version 1)
Die Spielwelt ist in viele kleine Karten aufgeteilt, welche entweder zu einem Spieler gehören
oder aber von der Spielleitung gestellt werden (Städte zum Beispiel).

#### Farmen
Farmen sind Karten, welche einem Spieler gehören und von diesem bebaut werden können.

#### Städte
In Städten gibt es Läden, welche von NPCs und Spielern betrieben werden können. Zudem gibt
es einen zentralen Händler, welcher die Waren, welche von Spielern produziert wurden, immer
ankauft.






## Begriffslexikon

### Tick
Ein periodisches Event, welches verwendet werden kann, um fortlaufende Prozesse zu bearbeiten.

## Comments and Thought Collections

An sich würde ich alle bisherigen Features von Harvest Moon übernehmen (Vorallem Dinge, die Back To Nature hat, meiner Meinung nach das beste HM-Spiel aller Zeiten) und die Zusätzlichen, bzw. meine Ideen dazu wären:
- Eine Online-Version:
  - Jeder hat eine eigene Farmfläche
  - Kann sein Haus, inklusive sein Interieu selbst gestalten, mit mehr Möbel-Auswahl
  - Das Chatten, da es dann online wäre dürfte nicht fehlen
  - Mehr Kleidungsauswahl und grundlegende Sprite-Gestaltung
  - Sämtliche Interaktionen die in Sims-Spielen zur Verfügung stehen übernehmen
  - Evtl. die Möglichkeit einen kleinen Handel, bzw. Laden in Game mit eigenen Preisen in Game zu führen
  - Dass der Zutritt nicht gewährleistet ist auf die Farm, wenn man es nicht möchte
  - Dass die Stadt wie z.B. in Back To Nature bleibt, nur etwas größer wird und evtl. andere Orte
  - Eine Sektion, wo man mit seinem friedlichen Charakter auch mal bei bestimmten Orten Kriege gegeneinander führen kann oder Autorennen, evtl. allg. ein Bereich wo man wie in GTA mit Fahrzeugen und Waffen rumläuft
  - Und Erweiterungen des Games nicht ausschließen und es immer weiter entwickeln

- Dabei ist auch zu sagen, dass man dann auch direkt ein Pokemon Online programmieren könnte der die genannten Feature beinhaltet, wie auch das Kochen und eigenes Haus haben + samt Pokemone besitzen und mehr anstellen können wie im Anime. Das könnte z.B. den anderen Pokemon-Spielen eine gute Konkurrenz sein und für dieses Spiel könnte ich auch einige weitere gute Programmierer dazu holen denke ich, da es für wirklich einige von Interesse sein könnte.