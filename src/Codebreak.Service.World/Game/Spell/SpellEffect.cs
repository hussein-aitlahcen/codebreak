using Codebreak.Service.World.Manager;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Spell
{
//E[4] = {d: "Téléporte à une distance de #1 cases maximum.", c: -1, o: ""};
//E[5] = {d: "Fait reculer de #1 case(s)", c: -1, o: ""};
//E[6] = {d: "Fait avancer de #1 case(s)", c: 0, o: ""};
//E[7] = {d: "Divorcer le couple", c: 0, o: ""};
//E[8] = {d: "Echange les places de 2 joueurs", c: 0, o: ""};
//E[9] = {d: "Esquive #1% des coups en reculant de #2 case(s)", c: 0, o: ""};
//E[10] = {d: "Permet d\'utiliser l\'émoticon #3", c: 0, o: ""};
//E[13] = {d: "Change le temps de jeu d\'un joueur", c: 0, o: ""};
//E[34] = {d: "Débute une quête", c: 0, o: "/"};
//E[50] = {d: "Porter un joueur", c: 0, o: ""};
//E[51] = {d: "Jeter un joueur", c: 0, o: ""};
//E[77] = {d: "Vole #1{~1~2 à }#2 PM", c: 0, o: "", j: true};
//E[78] = {d: "Ajoute +#1{~1~2 à }#2 PM", c: 0, o: "+", j: true};
//E[79] = {d: "#3% dommages subis x#1, sinon soigné de x#2", c: 0, o: ""};
//E[81] = {d: "PDV rendus : #1{~1~2 à }#2", c: 0, o: "", j: true};
//E[82] = {d: "Vole #1{~1~2 à }#2 PDV (fixe)", c: 0, o: "", j: true};
//E[84] = {d: "Vole #1{~1~2 à }#2 PA", c: 0, o: "", j: true};
//E[85] = {d: "Dommages : #1{~1~2 à }#2% de la vie de l\'attaquant (eau)", c: 0, o: "", j: true, e: "W"};
//E[86] = {d: "Dommages : #1{~1~2 à }#2% de la vie de l\'attaquant (terre)", c: 0, o: "", j: true, e: "E"};
//E[87] = {d: "Dommages : #1{~1~2 à }#2% de la vie de l\'attaquant (air)", c: 0, o: "", j: true, e: "A"};
//E[88] = {d: "Dommages : #1{~1~2 à }#2% de la vie de l\'attaquant (feu)", c: 0, o: "", j: true, e: "F"};
//E[89] = {d: "Dommages : #1{~1~2 à }#2% de la vie de l\'attaquant (neutre)", c: 0, o: "", j: true, e: "N"};
//E[90] = {d: "Donne #1{~1~2 à }#2 % de sa vie", c: 0, o: "", j: true};
//E[91] = {d: "Vole #1{~1~2 à }#2 PDV (eau)", c: 0, o: "", t: true, j: true, e: "W"};
//E[92] = {d: "Vole #1{~1~2 à }#2 PDV (terre)", c: 0, o: "", t: true, j: true, e: "E"};
//E[93] = {d: "Vole #1{~1~2 à }#2 PDV (air)", c: 0, o: "", t: true, j: true, e: "A"};
//E[94] = {d: "Vole #1{~1~2 à }#2 PDV (feu)", c: 0, o: "", t: true, j: true, e: "F"};
//E[95] = {d: "Vole #1{~1~2 à }#2 PDV (neutre)", c: 0, o: "", t: true, j: true, e: "N"};
//E[96] = {d: "Dommages : #1{~1~2 à }#2 (eau)", c: 0, o: "", t: true, j: true, e: "W"};
//E[97] = {d: "Dommages : #1{~1~2 à }#2 (terre)", c: 0, o: "", t: true, j: true, e: "E"};
//E[98] = {d: "Dommages : #1{~1~2 à }#2 (air)", c: 0, o: "", t: true, j: true, e: "A"};
//E[99] = {d: "Dommages : #1{~1~2 à }#2 (feu)", c: 0, o: "", t: true, j: true, e: "F"};
//E[100] = {d: "Dommages : #1{~1~2 à }#2 (neutre)", c: 0, o: "", t: true, j: true, e: "N"};
//E[101] = {d: "PA perdus à la cible : #1{~1~2 à }#2", c: 1, o: "/", j: true};
//E[105] = {d: "Dommages réduits de  #1{~1~2 à }#2", c: 16, o: "+", j: true};
//E[106] = {d: "Renvoie un sort de niveau #2 maximum", c: 30, o: ""};
//E[107] = {d: "Dommages retournés : #1{~1~2 à }#2", c: 31, o: "", j: true};
//E[108] = {d: "PDV rendus : #1{~1~2 à }#2", c: 0, o: "", j: true};
//E[109] = {d: "Dommages pour le lanceur : #1{~1~2 à }#2", c: 0, o: "-", j: true};
//E[110] = {d: "+#1{~1~2 à }#2 en vie", c: 0, o: "+", j: true};
//E[111] = {d: "+#1{~1~2 à }#2 PA", c: 1, o: "+", j: true};
//E[112] = {d: "+#1{~1~2 à }#2 de dommages", c: 16, o: "+", j: true};
//E[113] = {d: "Double les dommages ou rend  #1{~1~2 à }#2 PDV", c: 0, o: "", j: true};
//E[114] = {d: "Multiplie les dommages par #1", c: 17, o: "+"};
//E[115] = {d: "+#1{~1~2 à }#2 aux coups critiques", c: 18, o: "+", j: true};
//E[116] = {d: "-#1{~1~2 à -}#2 à la portée", c: 19, o: "-", j: true};
//E[117] = {d: "+#1{~1~2 à }#2 à la portée", c: 19, o: "+", j: true};
//E[118] = {d: "+#1{~1~2 à }#2 en force", c: 10, o: "+", j: true};
//E[119] = {d: "+#1{~1~2 à }#2 en agilité ", c: 14, o: "+", j: true};
//E[120] = {d: "Ajoute +#1{~1~2 à }#2 PA", c: 0, o: "+", j: true};
//E[121] = {d: "+#1{~1~2 à }#2 de dommages", c: 16, o: "+", j: true};
//E[122] = {d: "+#1{~1~2 à }#2 aux échecs critiques", c: 39, o: "-", j: true};
//E[123] = {d: "+#1{~1~2 à }#2 à la chance", c: 13, o: "+", j: true};
//E[124] = {d: "+#1{~1~2 à }#2 en sagesse", c: 12, o: "+", j: true};
//E[125] = {d: "+#1{~1~2 à }#2 en vitalité ", c: 11, o: "+", j: true};
//E[126] = {d: "+#1{~1~2 à }#2 en intelligence", c: 15, o: "+", j: true};
//E[127] = {d: "PM perdus : #1{~1~2 à }#2", c: 23, o: "-", j: true};
//E[128] = {d: "+#1{~1~2 à }#2 PM", c: 23, o: "+", j: true};
//E[130] = {d: "Vole  #1{~1~2 à }#2 d\'or", c: -1, o: "", j: true};
//E[131] = {d: "#1 PA utilisés font perdre #2 PDV", c: 0, o: "-"};
//E[132] = {d: "Enlève les envoûtements", c: -1, o: ""};
//E[133] = {d: "PA perdus pour le lanceur : #1{~1~2 à }#2", c: 1, o: "-", j: true};
//E[134] = {d: "PM perdus pour le lanceur : #1{~1~2 à }#2", c: 23, o: "-", j: true};
//E[135] = {d: "Portée du lanceur réduite de : #1{~1~2 à }#2", c: 19, o: "-", j: true};
//E[136] = {d: "Portée du lanceur augmentée de : #1{~1~2 à }#2", c: 19, o: "+", j: true};
//E[137] = {d: "Dommages physiques du lanceur augmentés de : #1{~1~2 à }#2", c: 16, o: "+", j: true};
//E[138] = {d: "Augmente les dommages de #1{~1~2 à }#2%", c: 17, o: "+", j: true};
//E[139] = {d: "Rend #1{~1~2 à }#2 points d\'énergie", c: 29, o: "+", j: true};
//E[140] = {d: "Fait passer le tour suivant", c: -1, o: ""};
//E[141] = {d: "Tue la cible", c: 0, o: ""};
//E[142] = {d: "+#1{~1~2 à }#2 aux dommages physiques", c: 16, o: "+", j: true};
//E[143] = {d: "PDV rendus : #1{~1~2 à }#2", c: 0, o: "", j: true};
//E[144] = {d: "Dommages : #1{~1~2 à }#2 (non boostés)", c: 0, o: "-", j: true};
//E[145] = {d: "-#1{~1~2 à }#2 aux dommages", c: 16, o: "-", j: true};
//E[146] = {d: "Change les paroles", c: 38, o: ""};
//E[147] = {d: "Ressuscite un allié ", c: 0, o: ""};
//E[148] = {d: "Quelqu\'un vous suit !", c: 0, o: ""};
//E[149] = {d: "Change l\'apparence", c: 38, o: ""};
//E[150] = {d: "Rend le personnage invisible", c: 24, o: "+"};
//E[152] = {d: "-#1{~1~2 à -}#2 en chance", c: 13, o: "-", j: true};
//E[153] = {d: "-#1{~1~2 à -}#2 en vitalité ", c: 11, o: "-", j: true};
//E[154] = {d: "-#1{~1~2 à -}#2 en agilité ", c: 14, o: "-", j: true};
//E[155] = {d: "-#1{~1~2 à -}#2 en intelligence", c: 15, o: "-", j: true};
//E[156] = {d: "-#1{~1~2 à -}#2 en sagesse", c: 12, o: "-", j: true};
//E[157] = {d: "-#1{~1~2 à -}#2 en force", c: 10, o: "-", j: true};
//E[158] = {d: "Augmente le poids portable de #1{~1~2 à }#2 pods", c: 0, o: "+", j: true};
//E[159] = {d: "Réduit le poids portable de #1{~1~2 à }#2 pods", c: 0, o: "-", j: true};
//E[160] = {d: "+#1{~1~2 à }#2 % de chance d\'esquiver les pertes de PA", c: 27, o: "+", j: true};
//E[161] = {d: "+#1{~1~2 à }#2 % de chance d\'esquiver les pertes de PM", c: 28, o: "+", j: true};
//E[162] = {d: "-#1{~1~2 à }#2 % de chance d\'esquiver les pertes de PA", c: 27, o: "-", j: true};
//E[163] = {d: "-#1{~1~2 à }#2 % de chance d\'esquiver les pertes de PM", c: 28, o: "-", j: true};
//E[164] = {d: "Dommages réduits de #1%", c: -1, o: "-"};
//E[165] = {d: "Augmente les dommages (#1) de #2%", c: 16, o: "+"};
//E[166] = {d: "PA retournés : #1{~1~2 à }#2", c: 0, o: "", j: true};
//E[168] = {d: "-#1{~1~2 à -}#2 PA", c: 1, o: "-", j: true};
//E[169] = {d: "-#1{~1~2 à -}#2 PM", c: 23, o: "-", j: true};
//E[171] = {d: "-#1{~1~2 à }#2 aux coups critiques", c: 18, o: "-", j: true};
//E[172] = {d: "Réduction magique diminué de #1{~1~2 à }#2", c: 20, o: "-", j: true};
//E[173] = {d: "Réduction physique diminué de #1{~1~2 à }#2", c: 21, o: "-", j: true};
//E[174] = {d: "+#1{~1~2 à }#2 en initiative", c: 44, o: "+", j: true};
//E[175] = {d: "-#1{~1~2 à }#2 en initiative", c: 44, o: "-", j: true};
//E[176] = {d: "+#1{~1~2 à }#2 en prospection", c: 48, o: "+", j: true};
//E[177] = {d: "-#1{~1~2 à }#2 en prospection", c: 48, o: "-", j: true};
//E[178] = {d: "+#1{~1~2 à }#2 de soins", c: 49, o: "+", j: true};
//E[179] = {d: "-#1{~1~2 à }#2 de soins", c: 49, o: "-", j: true};
//E[180] = {d: "Crée un double du lanceur de sort", c: -1, o: ""};
//E[181] = {d: "Invoque : #1", c: -1, o: ""};
//E[182] = {d: "+#1{~1~2 à }#2 créatures invocables", c: 26, o: "+", j: true};
//E[183] = {d: "Réduction magique de #1{~1~2 à }#2", c: 20, o: "/", j: true};
//E[184] = {d: "Réduction physique de #1{~1~2 à }#2", c: 21, o: "/", j: true};
//E[185] = {d: "Invoque une créature statique", c: 0, o: ""};
//E[186] = {d: "Diminue les dommages de #1{~1~2 à }#2%", c: 17, o: "-", j: true};
//E[188] = {d: "Changer l\'alignement", c: 0, o: "/"};
//E[194] = {d: "Gagner #1{~1~2 à }#2 kamas", c: 0, o: "+", j: true};
//E[197] = {d: "Transforme en #1", c: 0, o: ""};
//E[201] = {d: "Pose un objet au sol", c: -1, o: ""};
//E[202] = {d: "Dévoile tous les objets invisibles", c: -1, o: ""};
//E[206] = {d: "Ressuscite la cible", c: 0, o: ""};
//E[210] = {d: "#1{~1~2 à }#2 % de résistance à la terre", c: 33, o: "+", j: true};
//E[211] = {d: "#1{~1~2 à }#2 % de résistance à l\'eau", c: 35, o: "+", j: true};
//E[212] = {d: "#1{~1~2 à }#2 % de résistance à l\'air", c: 36, o: "+", j: true};
//E[213] = {d: "#1{~1~2 à }#2 % de résistance au feu", c: 34, o: "+", j: true};
//E[214] = {d: "#1{~1~2 à }#2 % de résistance neutre", c: 37, o: "+", j: true};
//E[215] = {d: "#1{~1~2 à }#2 % de faiblesse face à la terre", c: 33, o: "-", j: true};
//E[216] = {d: "#1{~1~2 à }#2 % de faiblesse face à l\'eau ", c: 35, o: "-", j: true};
//E[217] = {d: "#1{~1~2 à }#2 % de faiblesse face à l\'air", c: 36, o: "-", j: true};
//E[218] = {d: "#1{~1~2 à }#2 % de faiblesse face au feu", c: 34, o: "-", j: true};
//E[219] = {d: "#1{~1~2 à }#2 % de faiblesse neutre", c: 37, o: "-", j: true};
//E[220] = {d: "Renvoie #1 dommages", c: 50, o: "+"};
//E[221] = {d: "Qu\'y a-t-il là dedans ?", c: 0, o: ""};
//E[222] = {d: "Qu\'y a-t-il là dedans ?", c: 0, o: ""};
//E[225] = {d: "+#1{~1~2 à }#2 de dommages aux pièges", c: 70, o: "+", j: true};
//E[226] = {d: "+#1{~1~2 à }#2% de dommages aux pièges", c: 69, o: "+", j: true};
//E[229] = {d: "Récupère une monture !", c: 0, o: "+"};
//E[230] = {d: "+#1 en énergie perdue", c: 51, o: "/"};
//E[240] = {d: "+#1{~1~2 à }#2 de résistance à la terre", c: 54, o: "+", j: true};
//E[241] = {d: "+#1{~1~2 à }#2 de résistance à l\'eau", c: 56, o: "+", j: true};
//E[242] = {d: "+#1{~1~2 à }#2 de résistance à l\'air", c: 57, o: "+", j: true};
//E[243] = {d: "+#1{~1~2 à }#2 de résistance au feu", c: 55, o: "+", j: true};
//E[244] = {d: "+#1{~1~2 à }#2 de résistance neutre", c: 58, o: "+", j: true};
//E[245] = {d: "-#1{~1~2 à }#2 de résistance à la terre", c: 54, o: "-", j: true};
//E[246] = {d: "-#1{~1~2 à }#2 de résistance à l\'eau", c: 56, o: "-", j: true};
//E[247] = {d: "-#1{~1~2 à }#2 de résistance à l\'air", c: 57, o: "-", j: true};
//E[248] = {d: "-#1{~1~2 à }#2 de résistance au feu", c: 55, o: "-", j: true};
//E[249] = {d: "-#1{~1~2 à }#2 de résistance neutre", c: 58, o: "-", j: true};
//E[250] = {d: "#1{~1~2 à }#2 % de res. terre face aux combattants ", c: 59, o: "+", j: true};
//E[251] = {d: "#1{~1~2 à }#2 % de res. eau face aux combattants", c: 61, o: "+", j: true};
//E[252] = {d: "#1{~1~2 à }#2 % de res. air face aux combattants", c: 62, o: "+", j: true};
//E[253] = {d: "#1{~1~2 à }#2 % de res. feu face aux combattants", c: 60, o: "+", j: true};
//E[254] = {d: "#1{~1~2 à }#2 % de res. neutre face aux combattants", c: 63, o: "+", j: true};
//E[255] = {d: "#1{~1~2 à }#2 % de faiblesse terre face aux combattants", c: 59, o: "-", j: true};
//E[256] = {d: "#1{~1~2 à }#2 % de faiblesse eau face aux combattants", c: 61, o: "-", j: true};
//E[257] = {d: "#1{~1~2 à }#2 % de faiblesse air face aux combattants", c: 62, o: "-", j: true};
//E[258] = {d: "#1{~1~2 à }#2 % de faiblesse feu face aux combattants", c: 60, o: "-", j: true};
//E[259] = {d: "#1{~1~2 à }#2 % de faiblesse neutre face aux combattants", c: 63, o: "-", j: true};
//E[260] = {d: "+#1{~1~2 à }#2 de res. terre face aux combattants ", c: 64, o: "+", j: true};
//E[261] = {d: "+#1{~1~2 à }#2 de res. eau face aux combattants ", c: 66, o: "+", j: true};
//E[262] = {d: "+#1{~1~2 à }#2 de res. air face aux combattants", c: 67, o: "+", j: true};
//E[263] = {d: "+#1{~1~2 à }#2 de res. feu aux combattants", c: 65, o: "+", j: true};
//E[264] = {d: "+#1{~1~2 à }#2 de res. neutre aux combattants", c: 68, o: "+", j: true};
//E[265] = {d: "Dommages réduits de #1{~1~2 à }#2", c: 16, o: "+", j: true};
//E[266] = {d: "#1{~1~2 à -}#2 vol de chance", c: 13, o: "", j: true};
//E[267] = {d: "#1{~1~2 à -}#2 vol de vitalité ", c: 11, o: "", j: true};
//E[268] = {d: "#1{~1~2 à -}#2 vol d\'agilité ", c: 14, o: "", j: true};
//E[269] = {d: "#1{~1~2 à -}#2 vol d\'intelligence", c: 15, o: "", j: true};
//E[270] = {d: "#1{~1~2 à -}#2 vol de sagesse", c: 12, o: "", j: true};
//E[271] = {d: "#1{~1~2 à -}#2 vol de force", c: 12, o: "", j: true};
//E[275] = {d: "Dommages : #1{~1~2 à }#2% de la vie manquante de l\'attaquant (eau)", c: 0, o: "", j: true};
//E[276] = {d: "Dommages : #1{~1~2 à }#2% de la vie manquante de l\'attaquant (terre)", c: 0, o: "", j: true};
//E[277] = {d: "Dommages : #1{~1~2 à }#2% de la vie manquante de l\'attaquant (air)", c: 0, o: "", j: true};
//E[278] = {d: "Dommages : #1{~1~2 à }#2% de la vie manquante de l\'attaquant (feu)", c: 0, o: "", j: true};
//E[279] = {d: "Dommages : #1{~1~2 à }#2% de la vie manquante de l\'attaquant (neutre)", c: 0, o: "", j: true};
//E[281] = {d: "Augmente la portée du sort #1 de #3", c: 0, o: "+"};
//E[282] = {d: "Rend la portée du sort #1 modifiable", c: 0, o: "+"};
//E[283] = {d: "+#3 de dommages sur le sort #1", c: 0, o: "+"};
//E[284] = {d: "+#3 de soins sur le sort #1", c: 0, o: "+"};
//E[285] = {d: "Réduit de #3 le coût en PA du sort #1", c: 0, o: "+"};
//E[286] = {d: "Réduit de #3 le délai de relance du sort #1", c: 0, o: "+"};
//E[287] = {d: "+#3 aux CC sur le sort #1", c: 0, o: "+"};
//E[288] = {d: "Désactive le lancer en ligne du sort #1", c: 0, o: "+"};
//E[289] = {d: "Désactive la ligne de vue du sort #1", c: 0, o: "+"};
//E[290] = {d: "Augmente de #3 le nombre de lancer maximal par tour du sort #1", c: 0, o: "+"};
//E[291] = {d: "Augmente de #3 le nombre de lancer maximal par cible du sort #1", c: 0, o: "+"};
//E[292] = {d: "Fixe à #3 le délai de relance du sort #1", c: 0, o: "+"};
//E[293] = {d: "Augmente les dégâts de base du sort #1 de #3", c: 0, o: "+"};
//E[294] = {d: "Diminue la portée du sort #1 de #3", c: 0, o: "-"};
//E[310] = {d: "null", c: 0, o: "/"};
//E[320] = {d: "Vole #1{~1~2 à }#2 PO", c: 0, o: "", j: true};
//E[333] = {d: "Change une couleur", c: 38, o: ""};
//E[335] = {d: "Change l\'apparence", c: 0, o: ""};
//E[400] = {d: "Pose un piège de rang #2", c: -1, o: ""};
//E[401] = {d: "Pose un glyphe de rang #2", c: -1, o: ""};
//E[402] = {d: "Pose un glyphe de rang #2", c: -1, o: ""};
//E[405] = {d: "Tue et remplace par une invocation", c: 0, o: ""};
//E[406] = {d: "[wip]Enlève les effets du sort %1", c: 0, o: ""};
//E[407] = {d: "PDV rendus : #1{~1~2 à }#2", c: 0, o: "", j: true};
//E[513] = {d: "Pose un prisme", c: 0, o: ""};
//E[600] = {d: "Téléporte au point de sauvegarde", c: 0, o: ""};
//E[601] = {d: "null", c: 0, o: "+"};
//E[602] = {d: "Enregistre sa position", c: 0, o: ""};
//E[603] = {d: "Apprend le métier #3", c: 0, o: ""};
//E[604] = {d: "Apprend le sort #3", c: 0, o: ""};
//E[605] = {d: "+#1{~1~2 à }#2 points d\' XP", c: 0, o: "+", j: true};
//E[606] = {d: "+#1{~1~2 à }#2 en sagesse", c: 12, o: "+", j: true};
//E[607] = {d: "+#1{~1~2 à }#2 en force", c: 10, o: "+", j: true};
//E[608] = {d: "+#1{~1~2 à }#2 en chance", c: 13, o: "+", j: true};
//E[609] = {d: "+#1{~1~2 à }#2 en agilité ", c: 14, o: "+", j: true};
//E[610] = {d: "+#1{~1~2 à }#2 en vitalité ", c: 11, o: "+", j: true};
//E[611] = {d: "+#1{~1~2 à }#2 en intelligence", c: 15, o: "+", j: true};
//E[612] = {d: "+#1{~1~2 à }#2 points de caractéristique", c: 0, o: "+", j: true};
//E[613] = {d: "+#1{~1~2 à }#2 points de sort", c: 0, o: "+", j: true};
//E[614] = {d: "+ #1 d\'XP dans le métier #2", c: 0, o: "+"};
//E[615] = {d: "Fait oublier le métier de #3", c: 0, o: ""};
//E[616] = {d: "Fait oublier un niveau du sort #3", c: 0, o: ""};
//E[620] = {d: "Consulter #3", c: 0, o: ""};
//E[621] = {d: "Invoque : #3 (grade #1)", c: 0, o: ""};
//E[622] = {d: "Téléporte chez soi", c: 0, o: ""};
//E[623] = {d: "Invoque : #3", c: -1, o: "/"};
//E[624] = {d: "Fait oublier un niveau du sort #3", c: 0, o: ""};
//E[625] = {d: "null", c: 0, o: "/"};
//E[626] = {d: "null", c: 0, o: "/"};
//E[627] = {d: "Reproduit la carte d\'origine", c: 0, o: "/"};
//E[628] = {d: "Invoque : #3", c: 0, o: "/"};
//E[631] = {d: "null", c: 0, o: ""};
//E[640] = {d: "Ajoute #3 points d\'honneur", c: 52, o: "+"};
//E[641] = {d: "Ajoute #3 points de déshonneur", c: 53, o: "-"};
//E[642] = {d: "Retire #3 points d\'honneur", c: 52, o: "-"};
//E[643] = {d: "Retire #3 points de déshonneur", c: 53, o: "+"};
//E[645] = {d: "Ressuscite les alliés présents sur la carte", c: 0, o: "+"};
//E[646] = {d: "PDV rendus : #1{~1~2 à }#2", c: 0, o: "+", j: true};
//E[647] = {d: "Libère les âmes des ennemis", c: 0, o: "+"};
//E[648] = {d: "Libère une âme ennemie", c: 0, o: "+"};
//E[649] = {d: "Faire semblant d\'être #3", c: 0, o: "/"};
//E[654] = {d: "null", c: 0, o: "+"};
//E[666] = {d: "Pas d\'effet supplémentaire", c: 0, o: "/"};
//E[669] = {d: "Incarnation de niveau #3", c: 0, o: "/"};
//E[670] = {d: "Dommages : #1{~1~2 à }#2% de la vie de l\'attaquant (neutre)", c: 0, o: "", t: true, j: true};
//E[671] = {d: "Dommages : #1{~1~2 à }#2% de la vie de l\'attaquant (neutre)", c: 0, o: "", t: true, j: true};
//E[672] = {d: "Dommages : #1{~1~2 à }#2% de la vie de l\'attaquant (neutre)", c: 0, o: "", j: true};
//E[699] = {d: "Lier son métier : #1", c: 0, o: "/"};
//E[700] = {d: "Change l\'élément de frappe", c: 0, o: ""};
//E[701] = {d: "Puissance : #1{~1~2 à }#2", c: 0, o: ""};
//E[702] = {d: "+#1{~1~2 à }#2 point de durabilité ", c: 0, o: "", j: true};
//E[705] = {d: "#1% capture d\'âme de puissance #3", c: 0, o: "/"};
//E[706] = {d: "#1% de proba de capturer une monture", c: 0, o: "/"};
//E[710] = {d: "Coût supplémentaire", c: 0, o: "/"};
//E[715] = {d: "#1 : #3", c: 0, o: "/"};
//E[716] = {d: "#1 : #3", c: 0, o: "/"};
//E[717] = {d: "#1 : #3", c: 0, o: "/"};
//E[720] = {d: "Nombre de victimes : #2", c: 0, o: ""};
//E[724] = {d: "Débloque le titre #3", c: 0, o: ""};
//E[725] = {d: "Renommer la guilde : #4", c: 0, o: "/"};
//E[730] = {d: "Téléporte au prisme allié le plus proche", c: 0, o: ""};
//E[731] = {d: "Agresse les joueurs de l\'alignement ennemi automatiquement", c: 80, o: "+", j: true};
//E[732] = {d: "Résistance à l\'agression automatique par les joueurs ennemis : #1{~1~2 à }#2", c: 81, o: "+", j: true};
//E[740] = {d: "", c: 0, o: ""};
//E[741] = {d: "null", c: 0, o: ""};
//E[742] = {d: "null", c: 0, o: ""};
//E[750] = {d: "Bonus aux chances de capture : #1{~1~2 à }#2%", c: 72, o: "/", j: true};
//E[751] = {d: "Bonus à l\'xp de la dragodinde : #1{~1~2 à }#2%", c: 73, o: "/", j: true};
//E[752] = {d: "Bonus d\'esquive : #1{~1~2 à }#2%", c: 78, o: "+", j: true};
//E[753] = {d: "Bonus de blocage : #1{~1~2 à }#2%", c: 79, o: "+", j: true};
//E[760] = {d: "Disparaît en se déplaçant", c: 0, o: ""};
//E[765] = {d: "Echange les places de 2 joueurs", c: 0, o: ""};
//E[770] = {d: "Confusion horaire : #1{~1~2 à }#2 degrés", c: 74, o: "-", j: true};
//E[771] = {d: "Confusion horaire : #1{~1~2 à }#2 Pi/2", c: 74, o: "-", j: true};
//E[772] = {d: "Confusion horaire : #1{~1~2 à }#2 Pi/4", c: 74, o: "-", j: true};
//E[773] = {d: "Confusion contre horaire : #1{~1~2 à }#2 degrés", c: 74, o: "-", j: true};
//E[774] = {d: "Confusion contre horaire : #1{~1~2 à }#2 Pi/2", c: 74, o: "-", j: true};
//E[775] = {d: "Confusion contre horaire : #1{~1~2 à }#2 Pi/4", c: 74, o: "-", j: true};
//E[776] = {d: "+#1{~1~2 à }#2% de dégâts subis permanents", c: 75, o: "-", j: true};
//E[780] = {d: "Invoque le dernier allié mort avec #1{~1~2 à }#2 % de ses PDV", c: 0, o: "+", j: true};
//E[781] = {d: "Minimise les effets aléatoires", c: 0, o: "-"};
//E[782] = {d: "Maximise les effets aléatoires", c: 0, o: "/"};
//E[783] = {d: "Pousse jusqu\'à la case visée", c: 0, o: "/"};
//E[784] = {d: "Retour à la position de départ", c: 0, o: "/"};
//E[785] = {d: "null", c: 0, o: "/"};
//E[786] = {d: "Soigne sur attaque", c: 0, o: "/"};
//E[787] = {d: "#1", c: 0, o: "/"};
//E[788] = {d: "Châtiment de #2 sur #3 tour(s)", c: 0, o: "+"};
//E[789] = {d: "null", c: 0, o: "/"};
//E[790] = {d: "null", c: 0, o: ""};
//E[791] = {d: "Prépare #1{~1~2 à }#2 parchemins pour mercenaire [wait]", c: 0, o: "/"};
//E[792] = {d: "#1", c: 0, o: "/"};
//E[793] = {d: "#1", c: 0, o: "/"};
//E[795] = {d: "Arme de chasse", c: 0, o: "/"};
//E[800] = {d: "Points de vie : #3", c: 0, o: ""};
//E[805] = {d: "Reçu le : #1", c: 0, o: ""};
//E[806] = {d: "Corpulence : #1", c: 0, o: ""};
//E[807] = {d: "Dernier repas : #1", c: 0, o: ""};
//E[808] = {d: "A mangé le : #1", c: 0, o: ""};
//E[810] = {d: "Taille : #3 poces", c: 0, o: ""};
//E[811] = {d: "Tour(s) restant(s) : #3", c: 0, o: ""};
//E[812] = {d: "Résistance : #2 / #3", c: 0, o: ""};
//E[813] = {d: "null", c: 0, o: "/"};
//E[814] = {d: "#1", c: 0, o: "/"};
//E[815] = {d: "null", c: 0, o: ""};
//E[816] = {d: "null", c: 0, o: "/"};
//E[825] = {d: "Téléporte", c: 0, o: "+"};
//E[905] = {d: "Lance un combat contre #2", c: 0, o: "/"};
//E[930] = {d: "Augmente la sérénité, diminue l\'agressivité ", c: 0, o: "/"};
//E[931] = {d: "Augmente l\'agressivité, diminue la sérénité ", c: 0, o: "/"};
//E[932] = {d: "Augmente l\'endurance", c: 0, o: "+"};
//E[933] = {d: "Diminue l\'endurance", c: 0, o: "-"};
//E[934] = {d: "Augmente l\'amour", c: 0, o: "+"};
//E[935] = {d: "Diminue l\'amour", c: 0, o: "-"};
//E[936] = {d: "Accelère la maturité ", c: 0, o: "+"};
//E[937] = {d: "Ralentit la maturité ", c: 0, o: "-"};
//E[939] = {d: "Augmente les capacités d\'un familier #3", c: 0, o: "+"};
//E[940] = {d: "Capacités accrues", c: 0, o: "+"};
//E[946] = {d: "Retirer temporairement un objet d\'élevage", c: 0, o: "/"};
//E[947] = {d: "Récupérer un objet d\'enclos", c: 0, o: "/"};
//E[948] = {d: "Objet pour enclos", c: 0, o: "/"};
//E[949] = {d: "Monter/Descendre d\'une monture", c: 0, o: "/"};
//E[950] = {d: "Etat #3", c: 71, o: "/"};
//E[951] = {d: "Enlève l\'état \'#3\'", c: 71, o: "/"};
//E[952] = {d: "Désactive l\'état \'#3\'", c: 71, o: "/"};
//E[960] = {d: "Alignement : #3", c: 0, o: "/"};
//E[961] = {d: "Grade : #3", c: 0, o: "/"};
//E[962] = {d: "Niveau : #3", c: 0, o: "/"};
//E[963] = {d: "Créé depuis : #3 jour(s)", c: 0, o: "/"};
//E[964] = {d: "Nom : #4", c: 0, o: "/"};
//E[970] = {d: "null", c: 0, o: ""};
//E[971] = {d: "null", c: 0, o: ""};
//E[972] = {d: "null", c: 0, o: ""};
//E[973] = {d: "null", c: 0, o: ""};
//E[974] = {d: "null", c: 0, o: ""};
//E[981] = {d: "Non échangeable", c: 0, o: "/"};
//E[982] = {d: "Non échangeable", c: 0, o: "/"};
//E[983] = {d: "Échangeable dès le : #1", c: 0, o: "/"};
//E[984] = {d: "null", c: 0, o: "/"};
//E[985] = {d: "Modifié par : #4", c: 0, o: "/"};
//E[986] = {d: "Prépare #1{~1~2 à }#2 parchemins", c: 0, o: "/"};
//E[987] = {d: "Appartient à : #4", c: 0, o: "/"};
//E[988] = {d: "Fabriqué par : #4", c: 0, o: "/"};
//E[989] = {d: "Recherche : #4", c: 0, o: "/"};
//E[990] = {d: "#4", c: 0, o: "/"};
//E[994] = {d: "!! Certificat invalide !!", c: 0, o: "-"};
//E[995] = {d: "Consulter la fiche de la monture", c: 0, o: "/"};
//E[996] = {d: "Appartient à : #4", c: 0, o: "/"};
//E[997] = {d: "Nom : #4", c: 0, o: "/"};
//E[998] = {d: "Validité : #1j #2h #3m", c: 0, o: "/"};
//E[999] = {d: "null", c: 0, o: "+"};

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum EffectEnum : int
    {
        None = -1,

        // Armures
        AddArmorNeutral = -2,
        AddArmorEarth = -3,
        AddArmorFire = -4,
        AddArmorWater = -5,
        AddArmorAir = -6,

        Teleport = 4,
        PushBack = 5,
        PushFront = 6,
        Transpose = 8,
        Evasion = 9,
        DamageBrut = -7,

        TurnPass = 140,

        MPSteal = 77,
        MPBonus = 78,
        EcaflipChance = 79,
        LifeSteal = 82,
        APSteal = 84,
        ChanceSteal = 266,
        VitalitySteal = 267,
        AgilitySteal = 268,
        IntelligenceSteal = 269,
        WisdomSteal = 270,
        StrengthSteal = 271,

        DamageLifeWater = 85,
        DamageLifeEarth = 86,
        DamageLifeAir = 87,
        DamageLifeFire = 88,
        DamageLifeNeutral = 89,
        DamageDropLife = 90,
        StealWater = 91,
        StealEarth = 92,
        StealAir = 93,
        StealFire = 94,
        StealNeutral = 95,
        DamageWater = 96,
        DamageEarth = 97,
        DamageAir = 98,
        DamageFire = 99,
        DamageNeutral = 100,
        AddArmor = 105,
        AddArmorBis = 265,

        AddReflectDamageItem = 220,
        ReflectSpell = 106,
        AddReflectDamage = 107,
        Heal = 108,
        SelfDamage = 109,
        AddLife = 110,
        AddAP = 111,
        AddDamage = 112,
        MultiplyDamage = 114,

        AddAPBis = 120,
        AddAgility = 119,
        AddChance = 123,
        AddDamagePercent = 138,
        SubDamagePercent = 186,
        AddDamageCritic = 115,
        AddDamagePiege = 225,
        //AddDamagePiegePercent = 225,
        AddDamagePhysic = 142,
        AddDamageMagic = 143,
        AddEchecCritic = 122,
        AddAPDodge = 160,
        AddMPDodge = 161,
        AddStrength = 118,
        AddInitiative = 174,
        AddIntelligence = 126,
        AddInvocationMax = 182,
        AddMP = 128,
        AddPO = 117,
        AddPods = 158,
        AddProspection = 176,
        AddWisdom = 124,
        AddHealCare = 178,
        AddVitality = 125,
        SubAgility = 154,

        DamagePerAP = 131,
        IncreaseSpellDamage = 293,
        Mastery = 165,
        POSteal = 320,
        Punition = 672,
        Sacrifice = 765,

        SubChance = 152,
        SubDamage = 164,
        SubDamageCritic = 171,
        SubDamageMagic = 172,
        SubDamagePhysic = 173,
        SubAPDodge = 162,
        SubMPDodge = 163,
        SubStrength = 157,
        SubInitiative = 175,
        SubIntelligence = 155,
        SubAPDodgeable = 101,
        SubMPDodgeable = 127,
        SubAP = 168,
        SubMP = 169,
        SubPO = 116,
        SubPods = 159,
        SubProspection = 177,
        SubWisdom = 156,
        SubHealCare = 179,
        SubVitality = 153,

        InvocDouble = 180,
        Invocation = 181,

        AddReduceDamagePhysic = 183,
        AddReduceDamageMagic = 184,

        AddReduceDamagePercentWater = 211,
        AddReduceDamagePercentEarth = 210,
        AddReduceDamagePercentAir = 212,
        AddReduceDamagePercentFire = 213,
        AddReduceDamagePercentNeutral = 214,
        AddReduceDamagePercentPvPWater = 251,
        AddReduceDamagePercentPvPEarth = 250,
        AddReduceDamagePercentPvPAir = 252,
        AddReduceDamagePercentPvPFire = 253,
        AddReduceDamagePercentPvPNeutral = 254,

        AddReduceDamageWater = 241,
        AddReduceDamageEarth = 240,
        AddReduceDamageAir = 242,
        AddReduceDamageFire = 243,
        AddReduceDamageNeutral = 244,
        AddReduceDamagePvPWater = 261,
        AddReduceDamagePvPEarth = 260,
        AddReduceDamagePvPAir = 262,
        AddReduceDamagePvPFire = 263,
        AddReduceDamagePvPNeutral = 264,

        SubReduceDamagePercentWater = 216,
        SubReduceDamagePercentEarth = 215,
        SubReduceDamagePercentAir = 217,
        SubReduceDamagePercentFire = 218,
        SubReduceDamagePercentNeutral = 219,
        SubReduceDamagePercentPvPWater = 255,
        SubReduceDamagePercentPvPEarth = 256,
        SubReduceDamagePercentPvPAir = 257,
        SubReduceDamagePercentPvPFire = 258,
        SubReduceDamagePercentPvpNeutral = 259,
        SubReduceDamageWater = 246,
        SubReduceDamageEarth = 245,
        SubReduceDamageAir = 247,
        SubReduceDamageFire = 248,
        SubReduceDamageNeutral = 249,

        PandaCarrier = 50,
        PandaLaunch = 51,
        Perception = 202,
        ChangeSkin = 149,
        SpellBoost = 293,
        UseTrap = 400,
        UseGlyph = 401,
        DoNothing = 666,
        PushFear = 783,
        AddChatiment = 788,
        AddState = 950,
        RemoveState = 951,
        Stealth = 150,
        DeleteAllBonus = 132,

        /* Potion rappel */
        TeleportSavedZaap = 600,

        /* Parchemins */
        AddSpell = 604,
        AddExperience = 605,
        AddCaractStrength = 607,
        AddCaractWisdom = 678,
        AddCaractChance = 608,
        AddCaractAgility = 609,
        AddCaractVitality = 610,
        AddCaractIntelligence = 611,
        AddCaractPoint = 612,
        AddSpellPoint = 613,

        /* Gateaux */
        AddEnergy = 139,

        /* Bourses */
        AddKamas = 194,

        /* Bonbons boosts combats */
        AddBoost = 811,

        InvocationInformations = 628,

        SoulStoneStats = 705,
        MountCaptureProba = 706,

        SoulCaptureBonus = 750,
        MountExpBonus = 751,

        LastEat = 808,

        AlignmentId = 960,
        AlignmentGrade = 961,
        TargetLevel = 962,
        CreateTime = 963,
        TargetName = 964,
        
        LivingGfxId = 970,
        LivingMood = 971,
        LivingSkin = 972,
        LivingType = 973,
        LivingXp = 974,

        CanBeExchange = 983,

        MountOwner = 995,
        Name = 997,

        /* ACTION SPECIALES BDD SCRIPT */
        BddDialogReply = 2000,
        BddDialogLeave = 2001,
        BddOpenBank = 2002,
        BddAddStatistic = 2003,
        BddAddItem = 2004,
        BddTeleport = 2005,
    }

    /// <summary>
    /// 
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class SpellEffect
    {
        public int SpellId;
        public int SpellLevel;
        public int Type;
        public int Value1;
        public int Value2;
        public int Value3;
        public int Duration;
        public int Chance;
        private SpellLevel _level;

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public EffectEnum TypeEnum
        {
            get
            {
                return (EffectEnum)Type;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public SpellLevel Level
        {
            get
            {
                if(_level == null)
                    _level = SpellManager.Instance.GetSpellLevel(SpellId, SpellLevel);
                return _level;
            }
        }
    }
}
