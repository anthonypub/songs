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
mychords = \chords {                              c1:m                    | c2:m bes2     | aes1                    | aes2 g2       | f1                 | f1   | c1:m                         | bes1 }
music = \relative  { \key c \minor \tempo 4 = 130 r2 ees'16 g8. c16 ees8. | r2 r4 r8. d16 |  c8. aes16 ees8. r16 r2 | r2 r4 r8. f16 | a8. c16 f8. r16 r2 | r1   | r8 g16 ees8. c16 bes8. r8 r8. f'16 | d8. bes16 f8. r16 r2 }

\markup "Intro (after 4 bars drums), 1x full pattern"
\score {
  <<
    \mychords
    \music
  >>
  \layout {}
  \midi {}
}

\markup "Instrumental1"
penta = \relative { \key c \minor \tempo 4=130 \time 12/8 
                    | c'4. ~ c8 bes8 c ees4. ~ ees8 c8 ees | f g f ees c bes c4.~ c8 g bes 
                             | c4. ~ c8 bes8 c ees4. ~ ees8 c8 ees | f g f ees c ees f g f ees c bes | c1
}

\score {
  \penta
  \layout {}
  \midi {}
}

\markup "Instrumental2"
pentatoo = \relative { \key c \minor \tempo 4=130 \time 12/8 
                    | c'4. ~ c8 bes8 c ees4. ~ ees8 c8 ees | f g f ees c bes c4.~ c8 g bes 
                             | c4. ~ c8 bes8 c ees4. ~ ees8 c8 ees | f g f ees c ees g4. ~ g8 f g
                             | bes4. ~ bes8 g bes c4. ~ c8 bes c | ees c bes g bes c ees4. bes8 c4 
                             | ees8 c bes c bes g bes g f g f ees | g f ees f ees d f ees d ees d bes | c1 
                             
}

\score {
  \pentatoo
  \layout {}
  \midi {}
}

