// Learn more about F# at http://fsharp.org
open System


 type Coords = {                                        //used to store all information needed about each coordinate (or board space) on the board
    Pos: char * int                                     //the actual coordinates of the board space eg. (A,1)
    Layer: int                                          //which 'square' on the board this board space is found in (inner block = 1, middle block = 2 and outer block =3)
    Symbol: char                                        //the symbol currently shown as that board space (0 means unoccupied, anything else is a player's tile)
    PossibleMoves: (char * int) list                    //all posible board spaces a tile can be moved to when moving from this coordinate
  }

type PlayerState =                                      //the phase of the game the player is in
| FLYING                                                //NOTE: this phase is for when number of pieces on the board has decreased to 3
| MOVING                                                //NOTE: this phase is for once all pieces have been placed
| PLACING                                               //initial phase for placing tiles only

type Player =  {                                        //used to store player stats and their current game state (the phase they're in, the positions of their cows etc)
    Name: string                                        //The name of the player (as they've entered)
    ID: int                                             //The ID of the player (used to distinguish between the two)
    Symbol: char                                        //The symbol used to represent this player's cows on the board
    NumberOfPieces : int                                //The number of unplayed pieces they have left off the board
    PlayerState: PlayerState                            //Which phase of the game the player is in
    Positions: Coords List                              //The list of all coordinates the player has a cow placed
  }

type Mil = {                                            //used for creating all possible mills that can be formed (a mill is 3 coordinates in a straight line)
    Coords: Coords list;                                //list to store the 3 coordinates that create a mill
  }

let Player_1 = { Name = "Player 1"; ID = 1 ; Symbol = 'x'; NumberOfPieces = 12; PlayerState = PLACING; Positions = [] }             //Player 1's initial data
let Player_2 = { Name = "Player 2"; ID = 2 ; Symbol = 'o'; NumberOfPieces = 12; PlayerState = PLACING; Positions = [] }             //Player 2's initial data

let Players = [Player_1;Player_2]                                                                                                   //list of the two players (used to alternate between the two players depending on whose turn it is)

//  let removePiece (player:Player) (piece:Coord)= //remove play
let makeMove (coord:char*int) availableBoard = //try make a move using the available board
    (List.filter (fun x -> x.Pos=coord) availableBoard).Length <> 0; //if the length is not 0 this means a position that you tried to move to was taken
   
let decrementPieces (player:Player) = {player with NumberOfPieces = (player.NumberOfPieces - 1) }                                   //decreases the number of unplayed pieces a given player has by 1
let changePlayerState (player:Player) newState = { player with PlayerState = newState }                                             //sets the given player's state to the given state
let addPiece (player:Player) (piece:Coords) = {player with Positions = player.Positions@[piece] }                                   //adds a given coordinate to the list of positions the given player has cows placed (sets that coordinate to have a cow placed there by this player)
let movePiece (player:Player) (from:char*int) (to_:char*int)=  //assumes coords passed are valid
    { player with Positions = List.map (fun x -> match x.Pos=from with 
                                                  | true -> { x with Pos = to_} 
                                                  | _ -> x ) player.Positions  } //return the player with new positions

  
let A1 = {Pos=('A',1);Layer=3;Symbol=' ';PossibleMoves=[('A',4);('B',2);('D',1)] }                                                  //
let A4 = {Pos=('A',4);Layer=3;Symbol=' ';PossibleMoves=[('A',1);('A',7);('B',4)] } 
let A7 = {Pos=('A',7);Layer=3;Symbol=' ';PossibleMoves=[('A',4);('B',6);('D',7)] }

let B2 = {Pos=('B',2);Layer=2;Symbol=' ';PossibleMoves=[('A',1);('B',4);('C',3);('D',2)] }
let B4 = {Pos=('B',4);Layer=2;Symbol=' ';PossibleMoves=[('A',4);('B',2);('B',6);('C',4)] }
let B6 = {Pos=('B',6);Layer=2;Symbol=' ';PossibleMoves=[('A',7);('B',4);('D',6);('C',5)] }

let C3 = {Pos=('C',3);Layer=1;Symbol=' ';PossibleMoves=[('B',2);('C',4);('D',3)] }
let C4 = {Pos=('C',4);Layer=1;Symbol=' ';PossibleMoves=[('B',4);('C',3);('C',5)] }
let C5 = {Pos=('C',5);Layer=1;Symbol=' ';PossibleMoves=[('B',6);('C',4);('D',5)] }

let D1 = {Pos=('D',1);Layer=3;Symbol=' ';PossibleMoves=[('A',1);('D',2);('G',1)] }
let D2 = {Pos=('D',2);Layer=2;Symbol=' ';PossibleMoves=[('B',2);('D',1);('D',3);('F',2)] }
let D3 = {Pos=('D',3);Layer=1;Symbol=' ';PossibleMoves=[('C',3);('D',2);('E',3)] }

let D5 = {Pos=('D',5);Layer=1;Symbol=' ';PossibleMoves=[('C',5);('D',6);('E',5)] }
let D6 = {Pos=('D',6);Layer=2;Symbol=' ';PossibleMoves=[('B',6);('D',5);('D',7);('F',6)] }
let D7 = {Pos=('D',7);Layer=3;Symbol=' ';PossibleMoves=[('A',7);('D',6);('G',7)] }

let E3 = {Pos=('E',3);Layer=1;Symbol=' ';PossibleMoves=[('D',3);('F',2);('E',4)] }
let E4 = {Pos=('E',4);Layer=1;Symbol=' ';PossibleMoves=[('E',3);('F',4);('E',5)] }
let E5 = {Pos=('E',5);Layer=1;Symbol=' ';PossibleMoves=[('D',5);('E',4);('F',6)] }

let F2 = {Pos=('F',2);Layer=2;Symbol=' ';PossibleMoves=[('D',2);('E',3);('F',4);('G',1)] }
let F4 = {Pos=('F',4);Layer=2;Symbol=' ';PossibleMoves=[('E',4);('F',2);('F',6);('G',4)] }
let F6 = {Pos=('F',6);Layer=2;Symbol=' ';PossibleMoves=[('D',6);('E',5);('F',4);('G',7)] }

let G1 = {Pos=('G',1);Layer=3;Symbol=' ';PossibleMoves=[('D',1);('F',2);('G',4)] }
let G4 = {Pos=('G',4);Layer=3;Symbol=' ';PossibleMoves=[('F',4);('G',1);('G',7)] }
let G7 = {Pos=('G',7);Layer=3;Symbol=' ';PossibleMoves=[('D',7);('F',6);('G',4)] }


let startBoard=[A1;A4;A7;B2;B4;B6;C3;C4;C5;D1;D2;D3;D5;D6;D7;E3;E4;E5;F2;F4;F6;G1;G4;G7]
//let mills = [{Coords=[A1;B2;C3] M};[];[];[]] //make this a list of all the possible mills that can be formed
//player will check if there positions contains this mills[0] or mills[1]..

let printBoard (board:Coords list) (players:Player list) = //print a board b
    let ps1, ps2, posOfplayer1 =
        match players.[0].ID with
        | 1 -> '*',' ',0
        | _ -> ' ','*',1
     

    let boardString = sprintf  """
      1   2   3       4      5   6   7
 
  A  (%c)-------------(%c)------------(%c)
      |\              |             /|
      | \             |            / |
      |  \            |           /  |
  B   |  (%c)---------(%c)--------(%c)  |
      |   |\          |         /|   |
      |   | \         |        / |   |
      |   |  \        |       /  |   |
  C   |   |  (%c)-----(%c)----(%c)  |   |          %cPlayer 1               %cPlayer 2
      |   |   |              |   |   |          ----------              ----------
      |   |   |              |   |   |          Unplaced Cows : %d       Unplaced Cows : %d
  D  (%c)-(%c)-(%c)            (%c)-(%c)-(%c)         Cows alive :            Cows alive : 
      |   |   |              |   |   |          Cows killed :           Cows killed : 
      |   |   |              |   |   |
  E   |   |  (%c)-----(%c)----(%c)  |   |  
      |   |  /        |       \  |   |
      |   | /         |        \ |   |
      |   |/          |         \|   |
  F   |  (%c)---------(%c)--------(%c)  |
      |  /            |           \  |
      | /             |            \ |
      |/              |             \|
  G  (%c)-------------(%c)------------(%c)
   """ 
                        board.[0].Symbol board.[1].Symbol board.[2].Symbol board.[3].Symbol board.[4].Symbol board.[5].Symbol board.[6].Symbol board.[7].Symbol
                        board.[8].Symbol ps1 ps2 players.[posOfplayer1].NumberOfPieces players.[(posOfplayer1+1)%2].NumberOfPieces board.[9].Symbol board.[10].Symbol 
                        board.[11].Symbol board.[12].Symbol board.[13].Symbol board.[14].Symbol board.[15].Symbol board.[16].Symbol board.[17].Symbol board.[18].Symbol
                        board.[19].Symbol board.[20].Symbol board.[21].Symbol board.[22].Symbol board.[23].Symbol
    printf "%s" boardString

let filterOutBoard (filterBoard:(Coords list)) (boardToFilterOut:(Coords List)) = //returns coords in boardToFilterOut that aren't in filterBoard
    let rec filterOut (xs:Coords list) out = 
        match xs with
        | []->out
        | a::rest-> filterOut rest (List.filter (fun x -> x.Pos<>a.Pos ) out )
    filterOut filterBoard boardToFilterOut  //filterOut


let getCurrentBoard (playerPositions:(Coords list))  =
    let rec changeBoard (xs:Coords list) (out:Coords list) = 
        match xs with
        | []->out
        | a::rest-> changeBoard rest (List.map (fun x -> 
            match x.Pos=a.Pos with
            | true -> {x with Symbol = a.Symbol}
            | _ -> x) out ) 
             
    changeBoard playerPositions startBoard  //filterOut


let getCoords (pos:char*int) = //get the Coord type given pos and character to fill it with
    (List.filter (fun x-> x.Pos=pos)startBoard).[0]
     
let getPlayerMove (player:Player) : (char*int) =
    match player.PlayerState with
    | PLACING -> 
        printfn "%s's turn(%d pieces left)" player.Name player.NumberOfPieces
        printf "Row:" 
        let row= Char.ToUpper(Console.ReadLine().[0])
        printf "Column: " 
        let col = int (Console.ReadLine())
        (row,col)
    | MOVING -> ('A',0) //placeholder
    | FLYING -> ('A',0) //placeholder
   // | _ -> failwith "Game is bugged!"


  //getAvailableBoard 
  //get the avaibleBoard given both player lists of positions
  //check playerState of whichPlayer
  //get input from whichPlayer depending on state
  //check if input from user was valid
  //if move was valid move piece to desired location
  //change player to contain new list of positions placed
  //change numberofPieces and change state if neccessary

let rec runGame (players:Player list) = //pass message saying where player moved or that 
    //if error don't do next 3 lines error means move was not valid
    let currentBoard = getCurrentBoard (players.[0].Positions @ players.[1].Positions) //get the state of the board
    let availableBoard = filterOutBoard (players.[0].Positions @ players.[1].Positions) startBoard //the avaialble positions
    printBoard currentBoard players //print the board
    let pos= getPlayerMove players.[0]//the positon the player wants to move to
    let updatedPlayer=
        match makeMove pos availableBoard with //try to make a move to the available board
        | true -> 
                   addPiece players.[0] ({(getCoords pos) with Symbol = players.[0].Symbol }) 
                   |> decrementPieces  //move was valid
                   //use piping when using more functions on the player
                  
        | _ ->
            printfn "Pos is %A is not a valid position" pos
            runGame players //make the move again (will make more efficient later)
    
    runGame [players.[1];updatedPlayer]



let startGame () = 
    runGame Players //start the game

     
          

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    startGame ()
     //Console.ReadKey()
   
    //startGame ()
    0 // return an integer exit code




               
         