// Learn more about F# at http://fsharp.org
open System
open System.IO.Compression
open System.Security.Cryptography

//------------------------------data stucture-------------------------------------

 type Coords = {                                        //used to store all information needed about each coordinate (or board space) on the board
    Pos: char * int                                     //the actual coordinates of the board space eg. (A,1)                                      //which 'square' on the board this board space is found in (inner block = 1, middle block = 2 and outer block =3)
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

//------------------------------end data stucture-------------------------------------

//----------------------------hardcoded information-----------------------------------

let Player_1 = {                                                                            //Player 1's initial data
    Name = "Player 1"
    ID = 1 
    Symbol = 'x'
    NumberOfPieces = 12
    PlayerState = PLACING
    Positions = []
    }

let Player_2 = {                                                                            //Player 2's initial data
    Name = "Player 2"
    ID = 2
    Symbol = 'o'
    NumberOfPieces = 12
    PlayerState = PLACING
    Positions = []
    }

let A1 = {Pos=('A',1);Symbol=' ';PossibleMoves=[('A',4);('B',2);('D',1)] }                  //all individual board coordinates and their relevant data                          
let A4 = {Pos=('A',4);Symbol=' ';PossibleMoves=[('A',1);('A',7);('B',4)] } 
let A7 = {Pos=('A',7);Symbol=' ';PossibleMoves=[('A',4);('B',6);('D',7)] }

let B2 = {Pos=('B',2);Symbol=' ';PossibleMoves=[('A',1);('B',4);('C',3);('D',2)] }
let B4 = {Pos=('B',4);Symbol=' ';PossibleMoves=[('A',4);('B',2);('B',6);('C',4)] }
let B6 = {Pos=('B',6);Symbol=' ';PossibleMoves=[('A',7);('B',4);('D',6);('C',5)] }

let C3 = {Pos=('C',3);Symbol=' ';PossibleMoves=[('B',2);('C',4);('D',3)] }
let C4 = {Pos=('C',4);Symbol=' ';PossibleMoves=[('B',4);('C',3);('C',5)] }
let C5 = {Pos=('C',5);Symbol=' ';PossibleMoves=[('B',6);('C',4);('D',5)] }

let D1 = {Pos=('D',1);Symbol=' ';PossibleMoves=[('A',1);('D',2);('G',1)] }
let D2 = {Pos=('D',2);Symbol=' ';PossibleMoves=[('B',2);('D',1);('D',3);('F',2)] }
let D3 = {Pos=('D',3);Symbol=' ';PossibleMoves=[('C',3);('D',2);('E',3)] }

let D5 = {Pos=('D',5);Symbol=' ';PossibleMoves=[('C',5);('D',6);('E',5)] }
let D6 = {Pos=('D',6);Symbol=' ';PossibleMoves=[('B',6);('D',5);('D',7);('F',6)] }
let D7 = {Pos=('D',7);Symbol=' ';PossibleMoves=[('A',7);('D',6);('G',7)] }

let E3 = {Pos=('E',3);Symbol=' ';PossibleMoves=[('D',3);('F',2);('E',4)] }
let E4 = {Pos=('E',4);Symbol=' ';PossibleMoves=[('E',3);('F',4);('E',5)] }
let E5 = {Pos=('E',5);Symbol=' ';PossibleMoves=[('D',5);('E',4);('F',6)] }

let F2 = {Pos=('F',2);Symbol=' ';PossibleMoves=[('D',2);('E',3);('F',4);('G',1)] }
let F4 = {Pos=('F',4);Symbol=' ';PossibleMoves=[('E',4);('F',2);('F',6);('G',4)] }
let F6 = {Pos=('F',6);Symbol=' ';PossibleMoves=[('D',6);('E',5);('F',4);('G',7)] }

let G1 = {Pos=('G',1);Symbol=' ';PossibleMoves=[('D',1);('F',2);('G',4)] }
let G4 = {Pos=('G',4);Symbol=' ';PossibleMoves=[('F',4);('G',1);('G',7)] }
let G7 = {Pos=('G',7);Symbol=' ';PossibleMoves=[('D',7);('F',6);('G',4)] }

let startBoard = [ A1; A4; A7; B2; B4; B6; C3; C4; C5; D1; D2; D3; D5; D6; D7; E3; E4; E5; F2; F4; F6; G1; G4; G7 ] //the board consisting of all the coordinates


let AA17 = [A1; A4; A7]                                                                     //all coordinate combinations that can form a mill (if all are occupied by the same player)
let BB26 = [B2; B4; B6]
let CC35 = [C3; C4; C5]
let DD13 = [D1; D2; D3]
let DD57 = [D5; D6; D7]
let EE35 = [E3; E4; E5]
let FF26 = [F2; F4; F6]
let GG17 = [G1; G4; G7]

let AG11 = [A1; D1; G1]
let BF22 = [B2; D2; F2]
let CE33 = [C3; D3; E3]
let AC44 = [A4; B4; C4]
let EG44 = [E4; F4; G4]
let CE55 = [C5; D5; E5]
let BF66 = [B6; D6; F6]
let AG77 = [A7; D7; G7]

let AC13 = [A1; B2; C3]
let CA57 = [C5; B6; A7]
let GE13 = [G1; F2; E3]
let EG57 = [E5; F6; G7]


let allBoardMills = [ AA17; BB26; CC35; DD13; DD57; EE35; FF26; GG17; AG11; BF22; CE33; AC44; EG44; CE55; BF66; AG77; AC13; CA57; GE13; EG57 ] //list of all possible mill combinations

//---------------------------end hardcoded information-----------------------------------

//----------------------------------game code--------------------------------------------

let removePiece (player:Player) (piece:Coords) =
    { player with Positions = List.filter (fun x -> x.Pos<>piece.Pos) player.Positions }                                                //returns a new player the same as the given player, but with it's list of positions not containing the given coordinate
    
let isValidMove (coord:char*int) availableBoard = 
    List.exists (fun x -> x.Pos=coord) availableBoard                                                                                   //checks if the given coordinate is available (no cows are currently at that coordinate)
    
let decrementPieces (player:Player) =
    { player with NumberOfPieces = (player.NumberOfPieces - 1) }                                                                        //decreases the number of unplayed pieces that a given player has by 1

let changePlayerState (player:Player) newState =
    { player with PlayerState = newState }                                                                                              //sets the given player's state to the given state

let addPiece (player:Player) (piece:Coords) =
    { player with Positions = piece::player.Positions }                                                                                 //adds a given coordinate to the list of positions the given player has cows placed (sets that coordinate to have a cow placed there by this player)

let checkStateChange (player:Player) =                                                                                                  //checks if a given player satisfies the conditions which induce a change of state, and if they do, it returns a new player the same as the given player, but with the new state as it's state
    match player.PlayerState with
    | PLACING -> 
        match player.NumberOfPieces = 0 with 
        | true -> changePlayerState player MOVING
        | _ -> player 
    | MOVING -> 
        match player.Positions.Length = 3 with 
        | true -> changePlayerState player FLYING
        | _ -> player
    | _ -> player  
        
let printBoard (board:Coords list) (p1 : Player) (p2 : Player) =                                                                        //prints the board that represents the data given by a list of coordinates and information about the current players given by a player list
    System.Console.Clear ()
    let ps1, ps2, player1, player2 =
        match p1.ID with
        | 1 -> '*',' ', p1, p2
        | _ -> ' ','*', p2 , p1
                                                                                                                                        //the hardcoded board structure
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
  C   |   |  (%c)-----(%c)----(%c)  |   |          %cPlayer 1 (%c)           %cPlayer 2 (%c)
      |   |   |              |   |   |          ----------              ----------
      |   |   |              |   |   |          Unplaced Cows : %d      Unplaced Cows : %d
  D  (%c)-(%c)-(%c)            (%c)-(%c)-(%c)         Cows alive : %d          Cows alive : %d
      |   |   |              |   |   |          Cows killed : %d         Cows killed : %d
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
                        board.[0].Symbol board.[1].Symbol board.[2].Symbol board.[3].Symbol board.[4].Symbol board.[5].Symbol board.[6].Symbol board.[7].Symbol board.[8].Symbol ps1 player1.Symbol
                        ps2 player2.Symbol player1.NumberOfPieces player2.NumberOfPieces board.[9].Symbol board.[10].Symbol board.[11].Symbol board.[12].Symbol board.[13].Symbol board.[14].Symbol
                        player1.Positions.Length player2.Positions.Length (12-(player2.NumberOfPieces+player2.Positions.Length)) (12-(player1.NumberOfPieces+player1.Positions.Length))
                        board.[15].Symbol board.[16].Symbol board.[17].Symbol board.[18].Symbol board.[19].Symbol board.[20].Symbol board.[21].Symbol board.[22].Symbol board.[23].Symbol
    printf "%s" boardString

let filterOutBoard (filterBoard:(Coords list)) (boardToFilterOut:(Coords List)) =
    List.filter (fun x -> (List.filter (fun y -> x.Pos = y.Pos) filterBoard).Length = 0) boardToFilterOut                               //returns a list of coordinates that appear in a given 'boardToFilterOut' and not in a given 'filterBoard' (filters one board according to another)
    
let getAvailableBoard (board:Coords list):Coords list =
    List.filter (fun x -> x.Symbol = ' ') board                                                                                         //returns a list of coordinates that represent the epmty spaces on a given board (a given list of coordinates)
    
let getCurrentBoard (playerPositions:(Coords list)) =                                                                                   //returns a list of coordinates that represent the current board. In other words, it uses the given list (of the two player's coordinates lists combined) and fills in the unused coordinates with the relative coords from the start board.
    List.map (fun x -> 
        let k = (List.filter (fun y -> y.Pos = x.Pos) playerPositions)
        match k.Length with
        | 0 -> x
        | _ -> {x with Symbol  =  k.[0].Symbol} 
        ) startBoard
  
let getCoords (pos:char*int) =
    (List.filter (fun x-> x.Pos=pos)startBoard).[0]                                                                                     //returns the coordinate whose position is represented by the given tuple
    
let rec getPos what =                                                                                                                   //Asks for user input and returns the tuple of the coordinate they entered
    printfn "%s " what
    printf "Row:" 
    let rowInput = Console.ReadKey().KeyChar |> string
    let is_char, row = Char.TryParse(rowInput)
    printf "\nColumn: " 
    let getCol = Console.ReadKey().KeyChar |> string 
    printfn "" //for neatness 
    let is_numeric,_ = System.Int32.TryParse(getCol)
    match is_numeric && is_char with                                                                                                    //(check if user input for column is an int)
    | true -> (Char.ToUpper(row),(int getCol))
    | _ -> printfn "Row requires a character and Col requires a number. Please enter valid input "
           getPos what
     
let rec getPlayerMove (player:Player) availableBoard =                                                                                  //Begins the move of a given player. Requires a coords list of the current available board to help determine if the move is valid
    match player.PlayerState with
    | PLACING ->                                                                                                                        //(if the player is PLACING we are only interested in getting a toPos (where they want to place the cow))
        let toPos=getPos (sprintf "%s's turn\nWhere do you want to place the cow?" player.Name)
        match isValidMove toPos availableBoard with 
        | true ->  ('Z',100) , toPos   //return a random 'from' pos for placing state cause it's not needed                                                                                                //(if it is a valid move, return a tuple of a placeholder value and the tuple representing where the player wnats to place the cow)
        | _ -> printfn "%A is not a valid move" toPos
               getPlayerMove player availableBoard 
    | _ ->
        let fromPos= getPos (sprintf "%s's turn\nWhere do you want to move the cow from?" player.Name)
        match isValidMove fromPos player.Positions with
        | true -> 
            let toPos= getPos "Where do you want to move the cow to?"
            let playerMovingPositions = List.map (fun x -> getCoords x ) (getCoords fromPos).PossibleMoves
            match isValidMove toPos availableBoard && ( isValidMove toPos playerMovingPositions || player.PlayerState = FLYING ) with 
            | true -> fromPos, toPos
            | _ -> 
                printfn "You cannot move from %A to %A" fromPos toPos
                getPlayerMove player availableBoard
        | _ -> 
            printfn "You have no cow at %A" fromPos
            getPlayerMove player availableBoard 

let placeMove player pos =                                                                                                              //adds the given position to the given player's list of current positions and returns the updated player
    addPiece player ({(getCoords pos) with Symbol = player.Symbol }) 
    |> decrementPieces |> checkStateChange
                      
let movePiece (player:Player) (from:char*int) (toPos:char*int) = 
    let fromPos= getCoords from                                                                       //removes the 'from' coordinate and adds the 'to_' coordinate to the player's list of current coordinates and returns that updated player                            
    let newPositions =
        List.filter (fun (position:Coords) -> 
            position.Pos <> fromPos.Pos
            ) player.Positions
    { player with Positions = {getCoords toPos with Symbol = player.Symbol}::newPositions }

let getPlayerMills (player:Player) =                                                                                                    //returns a list of all mills the given player currently has
    List.filter (fun mill -> (filterOutBoard player.Positions mill).Length = 0) allBoardMills

let isInMill toPos playerMills =                                                                                                        //checks if the given position is within one of the given list of mills
    (List.filter (fun mill -> (List.filter (fun x -> x.Pos=toPos) mill).Length > 0) playerMills).Length>0

let canKillCowInMill (playerMills: list<Coords> list) (player: Player) = 
    (List.filter (fun x-> isInMill x.Pos playerMills) player.Positions).Length =player.Positions.Length
    

let rec killCow (player: Player) =                                                                                                      //asks the player which enemy cow they wish to remove and removes it (if it exists) else asks the player again until a valid cow is removed
    let pos = getPos "Which cow do you want to kill?"
    let playerMill=getPlayerMills player 
    match (isValidMove pos player.Positions),(isInMill pos playerMill),(canKillCowInMill playerMill player) with
    | true, true, true | true, false, _ -> removePiece player (getCoords pos)
    | true, true, _ -> 
        printfn "Can't kill cow in mill unless all cows are in mills" 
        killCow player
    | _ -> 
        printfn "No valid cow was in pos %A" pos
        killCow player 

let endGame (winner:Player)=
    printfn "Game has ended\n%s won, with %d cows still alive!" winner.Name winner.Positions.Length

let noMoveMessage (winner:Player) (loser:Player) =
    printfn "%s has no moves to play\n. Therefore %s wins the game with %d cows left." loser.Name winner.Name winner.Positions.Length

    // checks whether a player has any possible moves left to play
let moveExists (player: Player) (availableBoard: Coords List) =
    List.fold (fun intial (item:Coords) -> item.PossibleMoves @ intial) [] player.Positions // find all possible moves
    |> List.map getCoords // change to coordinate format
    |> List.exists (fun possiblePlayerMove -> List.exists ((=) possiblePlayerMove) availableBoard)

let rec runGame (p1 : Player) (p2 : Player) availableBoard currentBoard = //pass message saying where player moved or that 
    //let currentBoard = getCurrentBoard (p1.Positions @ p2.Positions) //get the state of the board
    //let availableBoard = getAvailableBoard currentBoard
    printBoard currentBoard p1 p2 //print the board
    let fromPos,toPos = getPlayerMove p1 availableBoard//the positon the player wants to move to
    let updatedPlayer =
        match p1.PlayerState with
        | PLACING -> placeMove p1 toPos 
        | _ -> movePiece p1 fromPos toPos // if flying or moving 
           
    let playerMills= getPlayerMills updatedPlayer
  
    let updatedEnemyPlayer=
        match isInMill toPos playerMills with
        | true -> 
                  printBoard (getCurrentBoard (updatedPlayer.Positions @ p2.Positions)) updatedPlayer p1
                  killCow p2 |> checkStateChange 
        | _ -> p2
    
    //check if game should end
    let newBoard = getCurrentBoard (updatedPlayer.Positions @ updatedEnemyPlayer.Positions)
    let newAvailableBoard = getAvailableBoard newBoard
    match updatedEnemyPlayer.Positions.Length, updatedEnemyPlayer, moveExists updatedEnemyPlayer newAvailableBoard with
    | 2, {PlayerState=FLYING}, _ -> endGame updatedPlayer//must be pure so let another function print        
    | _, {PlayerState=MOVING}, false -> noMoveMessage updatedPlayer updatedEnemyPlayer
    | _ ->  runGame updatedEnemyPlayer updatedPlayer newAvailableBoard newBoard  //change player turns

let startGame () = 
    runGame Player_1 Player_2 startBoard startBoard //start the game

let startMessage = 
    """
                            ___  ___                _                     _           
                            |  \/  |               | |                   | |          
                            | .  . | ___  _ __ __ _| |__   __ _ _ __ __ _| |__   __ _ 
                            | |\/| |/ _ \| '__/ _` | '_ \ / _` | '__/ _` | '_ \ / _` |
                            | |  | | (_) | | | (_| | |_) | (_| | | | (_| | |_) | (_| |
                            \_|  |_/\___/|_|  \__,_|_.__/ \__,_|_|  \__,_|_.__/ \__,_|
                                                          
                                             PRESS ANY KEY TO START              
                                             
                                                                                                      
                                                       
                                            /~~~~~\        /~~~~~\
                                           |    (~'        ~~~)   |
                                            \    \__________/    /
                                            /~::::::::         ~\
                                 /~~~~~~~-_| ::::::::             |_-~~~~~~~\
                                \ ======= /|  ::A::;      A     :|\ ====== /
                                 ~-_____-~ |  _----------------_::| ~-____-~
                                           |/~                  ~\|
                                           /                      \
                                          (        ()    ()        )
                                           `\                   ./'
                                             ~-_______________-~
                                                   /~~~~\
                                                  |      |
                                                  |      |
                                                 (________)    
                                                     ()      

    """
[<EntryPoint>]
let main argv =
    printfn "%s" startMessage
    Console.ReadKey()
    startGame ()
    Console.ReadKey()
    0 // return an integer exit code