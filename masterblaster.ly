\version "2.20.0"
\header {
  title = "Master Blaster"
  composer = "Stevie Wonder"
  tagline = \markup {
    Engraved at
    \simple #(strftime "%Y-%m-%d" (localtime (current-time)))
    with \with-url #"http://lilypond.org/"
    \line { LilyPond \simple #(lilypond-version) (http://lilypond.org/) }
  }
}
mychords = \chords { c1:m | c2:m bes2 | aes1 | aes2 g2 | f1 | f1 | c1:m | bes1 }
music = \relative { \key c \minor \tempo r = 130 r1 | r2 r2 | r1 | r2 r2 | r1 | r1 | r1 | r1 }
\score {
  <<
    \mychords
    \music
  >>
  \layout {}
  \midi {}
}
