Imports System.Numerics

Public Class KeyboardState
    Public Property Buttons as New BigInteger
    Public Shared H00 As BigInteger = New BigInteger(2^000)
    Public Shared H01 As BigInteger = New BigInteger(2^001)
    Public Shared H02 As BigInteger = New BigInteger(2^002)
    Public Shared H03 As BigInteger = New BigInteger(2^003)
    Public Shared H04 As BigInteger = New BigInteger(2^004)
    Public Shared H05 As BigInteger = New BigInteger(2^005)
    Public Shared H06 As BigInteger = New BigInteger(2^006)
    Public Shared H07 As BigInteger = New BigInteger(2^007)
    Public Shared H08 As BigInteger = New BigInteger(2^008)
    Public Shared H09 As BigInteger = New BigInteger(2^009)
    Public Shared H0A As BigInteger = New BigInteger(2^010)
    Public Shared H0B As BigInteger = New BigInteger(2^011)
    Public Shared H0C As BigInteger = New BigInteger(2^012)
    Public Shared H0D As BigInteger = New BigInteger(2^013)
    Public Shared H0E As BigInteger = New BigInteger(2^014)
    Public Shared H0F As BigInteger = New BigInteger(2^015)
    Public Shared H10 As BigInteger = New BigInteger(2^016)
    Public Shared H11 As BigInteger = New BigInteger(2^017)
    Public Shared H12 As BigInteger = New BigInteger(2^018)
    Public Shared H13 As BigInteger = New BigInteger(2^019)
    Public Shared H14 As BigInteger = New BigInteger(2^020)
    Public Shared H15 As BigInteger = New BigInteger(2^021)
    Public Shared H16 As BigInteger = New BigInteger(2^022)
    Public Shared H17 As BigInteger = New BigInteger(2^023)
    Public Shared H18 As BigInteger = New BigInteger(2^024)
    Public Shared H19 As BigInteger = New BigInteger(2^025)
    Public Shared H1A As BigInteger = New BigInteger(2^026)
    Public Shared H1B As BigInteger = New BigInteger(2^027)
    Public Shared H1C As BigInteger = New BigInteger(2^028)
    Public Shared H1D As BigInteger = New BigInteger(2^029)
    Public Shared H1E As BigInteger = New BigInteger(2^030)
    Public Shared H1F As BigInteger = New BigInteger(2^031)
    Public Shared H20 As BigInteger = New BigInteger(2^032)
    Public Shared H21 As BigInteger = New BigInteger(2^033)
    Public Shared H22 As BigInteger = New BigInteger(2^034)
    Public Shared H23 As BigInteger = New BigInteger(2^035)
    Public Shared H24 As BigInteger = New BigInteger(2^036)
    Public Shared H25 As BigInteger = New BigInteger(2^037)
    Public Shared H26 As BigInteger = New BigInteger(2^038)
    Public Shared H27 As BigInteger = New BigInteger(2^039)
    Public Shared H28 As BigInteger = New BigInteger(2^040)
    Public Shared H29 As BigInteger = New BigInteger(2^041)
    Public Shared H2A As BigInteger = New BigInteger(2^042)
    Public Shared H2B As BigInteger = New BigInteger(2^043)
    Public Shared H2C As BigInteger = New BigInteger(2^044)
    Public Shared H2D As BigInteger = New BigInteger(2^045)
    Public Shared H2E As BigInteger = New BigInteger(2^046)
    Public Shared H2F As BigInteger = New BigInteger(2^047)
    Public Shared H30 As BigInteger = New BigInteger(2^048)
    Public Shared H31 As BigInteger = New BigInteger(2^049)
    Public Shared H32 As BigInteger = New BigInteger(2^050)
    Public Shared H33 As BigInteger = New BigInteger(2^051)
    Public Shared H34 As BigInteger = New BigInteger(2^052)
    Public Shared H35 As BigInteger = New BigInteger(2^053)
    Public Shared H36 As BigInteger = New BigInteger(2^054)
    Public Shared H37 As BigInteger = New BigInteger(2^055)
    Public Shared H38 As BigInteger = New BigInteger(2^056)
    Public Shared H39 As BigInteger = New BigInteger(2^057)
    Public Shared H3A As BigInteger = New BigInteger(2^058)
    Public Shared H3B As BigInteger = New BigInteger(2^059)
    Public Shared H3C As BigInteger = New BigInteger(2^060)
    Public Shared H3D As BigInteger = New BigInteger(2^061)
    Public Shared H3E As BigInteger = New BigInteger(2^062)
    Public Shared H3F As BigInteger = New BigInteger(2^063)
    Public Shared H40 As BigInteger = New BigInteger(2^064)
    Public Shared H41 As BigInteger = New BigInteger(2^065)
    Public Shared H42 As BigInteger = New BigInteger(2^066)
    Public Shared H43 As BigInteger = New BigInteger(2^067)
    Public Shared H44 As BigInteger = New BigInteger(2^068)
    Public Shared H45 As BigInteger = New BigInteger(2^069)
    Public Shared H46 As BigInteger = New BigInteger(2^070)
    Public Shared H47 As BigInteger = New BigInteger(2^071)
    Public Shared H48 As BigInteger = New BigInteger(2^072)
    Public Shared H49 As BigInteger = New BigInteger(2^073)
    Public Shared H4A As BigInteger = New BigInteger(2^074)
    Public Shared H4B As BigInteger = New BigInteger(2^075)
    Public Shared H4C As BigInteger = New BigInteger(2^076)
    Public Shared H4D As BigInteger = New BigInteger(2^077)
    Public Shared H4E As BigInteger = New BigInteger(2^078)
    Public Shared H4F As BigInteger = New BigInteger(2^079)
    Public Shared H50 As BigInteger = New BigInteger(2^080)
    Public Shared H51 As BigInteger = New BigInteger(2^081)
    Public Shared H52 As BigInteger = New BigInteger(2^082)
    Public Shared H53 As BigInteger = New BigInteger(2^083)
    Public Shared H54 As BigInteger = New BigInteger(2^084)
    Public Shared H55 As BigInteger = New BigInteger(2^085)
    Public Shared H56 As BigInteger = New BigInteger(2^086)
    Public Shared H57 As BigInteger = New BigInteger(2^087)
    Public Shared H58 As BigInteger = New BigInteger(2^088)
    Public Shared H59 As BigInteger = New BigInteger(2^089)
    Public Shared H5A As BigInteger = New BigInteger(2^090)
    Public Shared H5B As BigInteger = New BigInteger(2^091)
    Public Shared H5C As BigInteger = New BigInteger(2^092)
    Public Shared H5D As BigInteger = New BigInteger(2^093)
    Public Shared H5E As BigInteger = New BigInteger(2^094)
    Public Shared H5F As BigInteger = New BigInteger(2^095)
    Public Shared H60 As BigInteger = New BigInteger(2^096)
    Public Shared H61 As BigInteger = New BigInteger(2^097)
    Public Shared H62 As BigInteger = New BigInteger(2^098)
    Public Shared H63 As BigInteger = New BigInteger(2^099)
    Public Shared H64 As BigInteger = New BigInteger(2^100)
    Public Shared H65 As BigInteger = New BigInteger(2^101)
    Public Shared H66 As BigInteger = New BigInteger(2^102)
    Public Shared H67 As BigInteger = New BigInteger(2^103)
    Public Shared H68 As BigInteger = New BigInteger(2^104)
    Public Shared H69 As BigInteger = New BigInteger(2^105)
    Public Shared H6A As BigInteger = New BigInteger(2^106)
    Public Shared H6B As BigInteger = New BigInteger(2^107)
    Public Shared H6C As BigInteger = New BigInteger(2^108)
    Public Shared H6D As BigInteger = New BigInteger(2^109)
    Public Shared H6E As BigInteger = New BigInteger(2^110)
    Public Shared H6F As BigInteger = New BigInteger(2^111)
    Public Shared H70 As BigInteger = New BigInteger(2^112)
    Public Shared H71 As BigInteger = New BigInteger(2^113)
    Public Shared H72 As BigInteger = New BigInteger(2^114)
    Public Shared H73 As BigInteger = New BigInteger(2^115)
    Public Shared H74 As BigInteger = New BigInteger(2^116)
    Public Shared H75 As BigInteger = New BigInteger(2^117)
    Public Shared H76 As BigInteger = New BigInteger(2^118)
    Public Shared H77 As BigInteger = New BigInteger(2^119)
    Public Shared H78 As BigInteger = New BigInteger(2^120)
    Public Shared H79 As BigInteger = New BigInteger(2^121)
    Public Shared H7A As BigInteger = New BigInteger(2^122)
    Public Shared H7B As BigInteger = New BigInteger(2^123)
    Public Shared H7C As BigInteger = New BigInteger(2^124)
    Public Shared H7D As BigInteger = New BigInteger(2^125)
    Public Shared H7E As BigInteger = New BigInteger(2^126)
    Public Shared H7F As BigInteger = New BigInteger(2^127)
    Public Shared H80 As BigInteger = New BigInteger(2^128)
    Public Shared H81 As BigInteger = New BigInteger(2^129)
    Public Shared H82 As BigInteger = New BigInteger(2^130)
    Public Shared H83 As BigInteger = New BigInteger(2^131)
    Public Shared H84 As BigInteger = New BigInteger(2^132)
    Public Shared H85 As BigInteger = New BigInteger(2^133)
    Public Shared H86 As BigInteger = New BigInteger(2^134)
    Public Shared H87 As BigInteger = New BigInteger(2^135)
    Public Shared H88 As BigInteger = New BigInteger(2^136)
    Public Shared H89 As BigInteger = New BigInteger(2^137)
    Public Shared H8A As BigInteger = New BigInteger(2^138)
    Public Shared H8B As BigInteger = New BigInteger(2^139)
    Public Shared H8C As BigInteger = New BigInteger(2^140)
    Public Shared H8D As BigInteger = New BigInteger(2^141)
    Public Shared H8E As BigInteger = New BigInteger(2^142)
    Public Shared H8F As BigInteger = New BigInteger(2^143)
    Public Shared H90 As BigInteger = New BigInteger(2^144)
    Public Shared H91 As BigInteger = New BigInteger(2^145)
    Public Shared H92 As BigInteger = New BigInteger(2^146)
    Public Shared H93 As BigInteger = New BigInteger(2^147)
    Public Shared H94 As BigInteger = New BigInteger(2^148)
    Public Shared H95 As BigInteger = New BigInteger(2^149)
    Public Shared H96 As BigInteger = New BigInteger(2^150)
    Public Shared H97 As BigInteger = New BigInteger(2^151)
    Public Shared H98 As BigInteger = New BigInteger(2^152)
    Public Shared H99 As BigInteger = New BigInteger(2^153)
    Public Shared H9A As BigInteger = New BigInteger(2^154)
    Public Shared H9B As BigInteger = New BigInteger(2^155)
    Public Shared H9C As BigInteger = New BigInteger(2^156)
    Public Shared H9D As BigInteger = New BigInteger(2^157)
    Public Shared H9E As BigInteger = New BigInteger(2^158)
    Public Shared H9F As BigInteger = New BigInteger(2^159)
    Public Shared HA0 As BigInteger = New BigInteger(2^160)
    Public Shared HA1 As BigInteger = New BigInteger(2^161)
    Public Shared HA2 As BigInteger = New BigInteger(2^162)
    Public Shared HA3 As BigInteger = New BigInteger(2^163)
    Public Shared HA4 As BigInteger = New BigInteger(2^164)
    Public Shared HA5 As BigInteger = New BigInteger(2^165)
    Public Shared HA6 As BigInteger = New BigInteger(2^166)
    Public Shared HA7 As BigInteger = New BigInteger(2^167)
    Public Shared HA8 As BigInteger = New BigInteger(2^168)
    Public Shared HA9 As BigInteger = New BigInteger(2^169)
    Public Shared HAA As BigInteger = New BigInteger(2^170)
    Public Shared HAB As BigInteger = New BigInteger(2^171)
    Public Shared HAC As BigInteger = New BigInteger(2^172)
    Public Shared HAD As BigInteger = New BigInteger(2^173)
    Public Shared HAE As BigInteger = New BigInteger(2^174)
    Public Shared HAF As BigInteger = New BigInteger(2^175)
    Public Shared HB0 As BigInteger = New BigInteger(2^176)
    Public Shared HB1 As BigInteger = New BigInteger(2^177)
    Public Shared HB2 As BigInteger = New BigInteger(2^178)
    Public Shared HB3 As BigInteger = New BigInteger(2^179)
    Public Shared HB4 As BigInteger = New BigInteger(2^180)
    Public Shared HB5 As BigInteger = New BigInteger(2^181)
    Public Shared HB6 As BigInteger = New BigInteger(2^182)
    Public Shared HB7 As BigInteger = New BigInteger(2^183)
    Public Shared HB8 As BigInteger = New BigInteger(2^184)
    Public Shared HB9 As BigInteger = New BigInteger(2^185)
    Public Shared HBA As BigInteger = New BigInteger(2^186)
    Public Shared HBB As BigInteger = New BigInteger(2^187)
    Public Shared HBC As BigInteger = New BigInteger(2^188)
    Public Shared HBD As BigInteger = New BigInteger(2^189)
    Public Shared HBE As BigInteger = New BigInteger(2^190)
    Public Shared HBF As BigInteger = New BigInteger(2^191)
    Public Shared HC0 As BigInteger = New BigInteger(2^192)
    Public Shared HC1 As BigInteger = New BigInteger(2^193)
    Public Shared HC2 As BigInteger = New BigInteger(2^194)
    Public Shared HC3 As BigInteger = New BigInteger(2^195)
    Public Shared HC4 As BigInteger = New BigInteger(2^196)
    Public Shared HC5 As BigInteger = New BigInteger(2^197)
    Public Shared HC6 As BigInteger = New BigInteger(2^198)
    Public Shared HC7 As BigInteger = New BigInteger(2^199)
    Public Shared HC8 As BigInteger = New BigInteger(2^200)
    Public Shared HC9 As BigInteger = New BigInteger(2^201)
    Public Shared HCA As BigInteger = New BigInteger(2^202)
    Public Shared HCB As BigInteger = New BigInteger(2^203)
    Public Shared HCC As BigInteger = New BigInteger(2^204)
    Public Shared HCD As BigInteger = New BigInteger(2^205)
    Public Shared HCE As BigInteger = New BigInteger(2^206)
    Public Shared HCF As BigInteger = New BigInteger(2^207)
    Public Shared HD0 As BigInteger = New BigInteger(2^208)
    Public Shared HD1 As BigInteger = New BigInteger(2^209)
    Public Shared HD2 As BigInteger = New BigInteger(2^210)
    Public Shared HD3 As BigInteger = New BigInteger(2^211)
    Public Shared HD4 As BigInteger = New BigInteger(2^212)
    Public Shared HD5 As BigInteger = New BigInteger(2^213)
    Public Shared HD6 As BigInteger = New BigInteger(2^214)
    Public Shared HD7 As BigInteger = New BigInteger(2^215)
    Public Shared HD8 As BigInteger = New BigInteger(2^216)
    Public Shared HD9 As BigInteger = New BigInteger(2^217)
    Public Shared HDA As BigInteger = New BigInteger(2^218)
    Public Shared HDB As BigInteger = New BigInteger(2^219)
    Public Shared HDC As BigInteger = New BigInteger(2^220)
    Public Shared HDD As BigInteger = New BigInteger(2^221)
    Public Shared HDE As BigInteger = New BigInteger(2^222)
    Public Shared HDF As BigInteger = New BigInteger(2^223)
    Public Shared HE0 As BigInteger = New BigInteger(2^224)
    Public Shared HE1 As BigInteger = New BigInteger(2^225)
    Public Shared HE2 As BigInteger = New BigInteger(2^226)
    Public Shared HE3 As BigInteger = New BigInteger(2^227)
    Public Shared HE4 As BigInteger = New BigInteger(2^228)
    Public Shared HE5 As BigInteger = New BigInteger(2^229)
    Public Shared HE6 As BigInteger = New BigInteger(2^230)
    Public Shared HE7 As BigInteger = New BigInteger(2^231)
    Public Shared HE8 As BigInteger = New BigInteger(2^232)
    Public Shared HE9 As BigInteger = New BigInteger(2^233)
    Public Shared HEA As BigInteger = New BigInteger(2^234)
    Public Shared HEB As BigInteger = New BigInteger(2^235)
    Public Shared HEC As BigInteger = New BigInteger(2^236)
    Public Shared HED As BigInteger = New BigInteger(2^237)
    Public Shared HEE As BigInteger = New BigInteger(2^238)
    Public Shared HEF As BigInteger = New BigInteger(2^239)
    Public Shared HF0 As BigInteger = New BigInteger(2^240)
    Public Shared HF1 As BigInteger = New BigInteger(2^241)
    Public Shared HF2 As BigInteger = New BigInteger(2^242)
    Public Shared HF3 As BigInteger = New BigInteger(2^243)
    Public Shared HF4 As BigInteger = New BigInteger(2^244)
    Public Shared HF5 As BigInteger = New BigInteger(2^245)
    Public Shared HF6 As BigInteger = New BigInteger(2^246)
    Public Shared HF7 As BigInteger = New BigInteger(2^247)
    Public Shared HF8 As BigInteger = New BigInteger(2^248)
    Public Shared HF9 As BigInteger = New BigInteger(2^249)
    Public Shared HFA As BigInteger = New BigInteger(2^250)
    Public Shared HFB As BigInteger = New BigInteger(2^251)
    Public Shared HFC As BigInteger = New BigInteger(2^252)
    Public Shared HFD As BigInteger = New BigInteger(2^253)
    Public Shared HFE As BigInteger = New BigInteger(2^254)
    Public Shared HFF As BigInteger = New BigInteger(2^255)

    Public Shared Function ScanToHex(Scancode As Long) As BigInteger
        Select Case Scancode
            Case 000 : Return H00
            Case 001 : Return H01
            Case 002 : Return H02
            Case 003 : Return H03
            Case 004 : Return H04
            Case 005 : Return H05
            Case 006 : Return H06
            Case 007 : Return H07
            Case 008 : Return H08
            Case 009 : Return H09
            Case 010 : Return H0A
            Case 011 : Return H0B
            Case 012 : Return H0C
            Case 013 : Return H0D
            Case 014 : Return H0E
            Case 015 : Return H0F
            Case 016 : Return H10
            Case 017 : Return H11
            Case 018 : Return H12
            Case 019 : Return H13
            Case 020 : Return H14
            Case 021 : Return H15
            Case 022 : Return H16
            Case 023 : Return H17
            Case 024 : Return H18
            Case 025 : Return H19
            Case 026 : Return H1A
            Case 027 : Return H1B
            Case 028 : Return H1C
            Case 029 : Return H1D
            Case 030 : Return H1E
            Case 031 : Return H1F
            Case 032 : Return H20
            Case 033 : Return H21
            Case 034 : Return H22
            Case 035 : Return H23
            Case 036 : Return H24
            Case 037 : Return H25
            Case 038 : Return H26
            Case 039 : Return H27
            Case 040 : Return H28
            Case 041 : Return H29
            Case 042 : Return H2A
            Case 043 : Return H2B
            Case 044 : Return H2C
            Case 045 : Return H2D
            Case 046 : Return H2E
            Case 047 : Return H2F
            Case 048 : Return H30
            Case 049 : Return H31
            Case 050 : Return H32
            Case 051 : Return H33
            Case 052 : Return H34
            Case 053 : Return H35
            Case 054 : Return H36
            Case 055 : Return H37
            Case 056 : Return H38
            Case 057 : Return H39
            Case 058 : Return H3A
            Case 059 : Return H3B
            Case 060 : Return H3C
            Case 061 : Return H3D
            Case 062 : Return H3E
            Case 063 : Return H3F
            Case 064 : Return H40
            Case 065 : Return H41
            Case 066 : Return H42
            Case 067 : Return H43
            Case 068 : Return H44
            Case 069 : Return H45
            Case 070 : Return H46
            Case 071 : Return H47
            Case 072 : Return H48
            Case 073 : Return H49
            Case 074 : Return H4A
            Case 075 : Return H4B
            Case 076 : Return H4C
            Case 077 : Return H4D
            Case 078 : Return H4E
            Case 079 : Return H4F
            Case 080 : Return H50
            Case 081 : Return H51
            Case 082 : Return H52
            Case 083 : Return H53
            Case 084 : Return H54
            Case 085 : Return H55
            Case 086 : Return H56
            Case 087 : Return H57
            Case 088 : Return H58
            Case 089 : Return H59
            Case 090 : Return H5A
            Case 091 : Return H5B
            Case 092 : Return H5C
            Case 093 : Return H5D
            Case 094 : Return H5E
            Case 095 : Return H5F
            Case 096 : Return H60
            Case 097 : Return H61
            Case 098 : Return H62
            Case 099 : Return H63
            Case 100 : Return H64
            Case 101 : Return H65
            Case 102 : Return H66
            Case 103 : Return H67
            Case 104 : Return H68
            Case 105 : Return H69
            Case 106 : Return H6A
            Case 107 : Return H6B
            Case 108 : Return H6C
            Case 109 : Return H6D
            Case 110 : Return H6E
            Case 111 : Return H6F
            Case 112 : Return H70
            Case 113 : Return H71
            Case 114 : Return H72
            Case 115 : Return H73
            Case 116 : Return H74
            Case 117 : Return H75
            Case 118 : Return H76
            Case 119 : Return H77
            Case 120 : Return H78
            Case 121 : Return H79
            Case 122 : Return H7A
            Case 123 : Return H7B
            Case 124 : Return H7C
            Case 125 : Return H7D
            Case 126 : Return H7E
            Case 127 : Return H7F
            Case 128 : Return H80
            Case 129 : Return H81
            Case 130 : Return H82
            Case 131 : Return H83
            Case 132 : Return H84
            Case 133 : Return H85
            Case 134 : Return H86
            Case 135 : Return H87
            Case 136 : Return H88
            Case 137 : Return H89
            Case 138 : Return H8A
            Case 139 : Return H8B
            Case 140 : Return H8C
            Case 141 : Return H8D
            Case 142 : Return H8E
            Case 143 : Return H8F
            Case 144 : Return H90
            Case 145 : Return H91
            Case 146 : Return H92
            Case 147 : Return H93
            Case 148 : Return H94
            Case 149 : Return H95
            Case 150 : Return H96
            Case 151 : Return H97
            Case 152 : Return H98
            Case 153 : Return H99
            Case 154 : Return H9A
            Case 155 : Return H9B
            Case 156 : Return H9C
            Case 157 : Return H9D
            Case 158 : Return H9E
            Case 159 : Return H9F
            Case 160 : Return HA0
            Case 161 : Return HA1
            Case 162 : Return HA2
            Case 163 : Return HA3
            Case 164 : Return HA4
            Case 165 : Return HA5
            Case 166 : Return HA6
            Case 167 : Return HA7
            Case 168 : Return HA8
            Case 169 : Return HA9
            Case 170 : Return HAA
            Case 171 : Return HAB
            Case 172 : Return HAC
            Case 173 : Return HAD
            Case 174 : Return HAE
            Case 175 : Return HAF
            Case 176 : Return HB0
            Case 177 : Return HB1
            Case 178 : Return HB2
            Case 179 : Return HB3
            Case 180 : Return HB4
            Case 181 : Return HB5
            Case 182 : Return HB6
            Case 183 : Return HB7
            Case 184 : Return HB8
            Case 185 : Return HB9
            Case 186 : Return HBA
            Case 187 : Return HBB
            Case 188 : Return HBC
            Case 189 : Return HBD
            Case 190 : Return HBE
            Case 191 : Return HBF
            Case 192 : Return HC0
            Case 193 : Return HC1
            Case 194 : Return HC2
            Case 195 : Return HC3
            Case 196 : Return HC4
            Case 197 : Return HC5
            Case 198 : Return HC6
            Case 199 : Return HC7
            Case 200 : Return HC8
            Case 201 : Return HC9
            Case 202 : Return HCA
            Case 203 : Return HCB
            Case 204 : Return HCC
            Case 205 : Return HCD
            Case 206 : Return HCE
            Case 207 : Return HCF
            Case 208 : Return HD0
            Case 209 : Return HD1
            Case 210 : Return HD2
            Case 211 : Return HD3
            Case 212 : Return HD4
            Case 213 : Return HD5
            Case 214 : Return HD6
            Case 215 : Return HD7
            Case 216 : Return HD8
            Case 217 : Return HD9
            Case 218 : Return HDA
            Case 219 : Return HDB
            Case 220 : Return HDC
            Case 221 : Return HDD
            Case 222 : Return HDE
            Case 223 : Return HDF
            Case 224 : Return HE0
            Case 225 : Return HE1
            Case 226 : Return HE2
            Case 227 : Return HE3
            Case 228 : Return HE4
            Case 229 : Return HE5
            Case 230 : Return HE6
            Case 231 : Return HE7
            Case 232 : Return HE8
            Case 233 : Return HE9
            Case 234 : Return HEA
            Case 235 : Return HEB
            Case 236 : Return HEC
            Case 237 : Return HED
            Case 238 : Return HEE
            Case 239 : Return HEF
            Case 240 : Return HF0
            Case 241 : Return HF1
            Case 242 : Return HF2
            Case 243 : Return HF3
            Case 244 : Return HF4
            Case 245 : Return HF5
            Case 246 : Return HF6
            Case 247 : Return HF7
            Case 248 : Return HF8
            Case 249 : Return HF9
            Case 250 : Return HFA
            Case 251 : Return HFB
            Case 252 : Return HFC
            Case 253 : Return HFD
            Case 254 : Return HFE
            Case 255 : Return HFF
            Case Else : Return HFF
        End Select
    End Function
End Class