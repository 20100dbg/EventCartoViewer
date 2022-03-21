# Introduction
EventCartoViewer est un outil pour visualiser des évènements géolocalisés à l'aide d'une timeline

Ces évènements peuvent êtres des points (fait ponctuel), lignes (itinéraires) ou surface (zones d'influence)

Un curseur permet de se déplacer dans le temps et d'observer l'évolution des évènements.


# Todo 
support de la durée d'un évenèment
- champs gdh start et gdh end

id, idShape, typeSHape ?, GDH, label, description, style
style = dégradé ?

Afficher valeurs min/max et start/end du slider

buffer autour des surfaces

surface et line
- créer dans l'ordre des points (ordre gdh)
- relier les points extérieur

format CSV
- WKT
- WKT simplifié ?
- une ligne par point (nécessite id et type de shape)

déterminer automatiquement l'unité de temps à utiliser

SetSlider : étendre de +/- 5 minutes aux extrémités