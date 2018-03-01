// Learn more about F# at http://fsharp.org
open System
 type Coords={
       Pos: char * int
       Layer: int
       Status: Option<char>
       PossibleMoves: (char * int) list
    }

type PlayerState =
| FLYING //number of pieces has decreased to 3
| MOVING  //all pieces have been placed
| PLACING

type Player =
  {
    Name: string
    Symbol: char
    NumberOfPieces : int
    PlayerState: PlayerState
    Positions: Coords List
  }
let Player_1 = { Name="Player 1";Symbol='1';NumberOfPieces= 12; PlayerState = PLACING;Positions = [] }
let Player_2 =  { Name="Player 2";Symbol='X'; NumberOfPieces= 12; PlayerState = PLACING; Positions = []}
let Players=[Player_1;Player_2];
//  let removePiece (player:Player) (piece:Coord)= //remove play
let decrementPieces (player:Player) = {player with NumberOfPieces=(player.NumberOfPieces - 1)}
let changePlayerState (player:Player) newState= { player with PlayerState = newState }
let addPiece (player:Player) (piece:Coords) = {player with Positions= player.Positions@[piece] }
 (*let movePieces (player:Player) (from:char*int) (to_:Coords)= 
    let rec checkList (xs:Coords list) out :(Coords List) =
        match xs with
        | [] ->out
        | a::rest ->
            match a.Pos with
            | from -> rest@[to_]; //remove what 'a' was and replace it with to_ (the new position)
            | _ -> checkList (rest@[a]) rest; //doesn't
    match from with 
    | 
    | *)


type MilStatus =
    | Broken
    | Formed
    | Free
type Mil =
    {
        Coords: Coords list;
        MilStatus: MilStatus
    }
let A1 = {Pos=('A',1);Layer=3;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let A4 = {Pos=('A',4);Layer=3;Status=None;PossibleMoves=[('A',1);('A',7);('B',4)] }  //done up to here
let A7 = {Pos=('A',7);Layer=3;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let B2 = {Pos=('B',2);Layer=2;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let B4 = {Pos=('B',4);Layer=2;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let B6 = {Pos=('B',6);Layer=2;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let C3 = {Pos=('C',3);Layer=1;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let C4 = {Pos=('C',4);Layer=1;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let C5 = {Pos=('C',5);Layer=1;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let D1 = {Pos=('D',1);Layer=3;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let D2 = {Pos=('D',2);Layer=2;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let D3 = {Pos=('D',3);Layer=1;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }
let D5 = {Pos=('D',5);Layer=1;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let D6 = {Pos=('D',6);Layer=2;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let D7 = {Pos=('D',7);Layer=3;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let E3 = {Pos=('E',3);Layer=1;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let E4 = {Pos=('E',4);Layer=1;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let E5 = {Pos=('E',5);Layer=1;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let F2 = {Pos=('F',2);Layer=2;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let F4 = {Pos=('F',4);Layer=2;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let F6 = {Pos=('F',6);Layer=2;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let G1 = {Pos=('G',1);Layer=3;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] };
let G4 = {Pos=('G',4);Layer=3;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }; 
let G7 = {Pos=('G',7);Layer=3;Status=None;PossibleMoves=[('A',4);('B',2);('D',1)] }; 


let startBoard=[A1;A4;A7;B2;B4;B6;C3;C4;C5;D1;D2;D3;D5;D6;D7;E3;E4;E5;F2;F4;F6;G1;G4;G7]
//let mills = [{Coords=[A1;B2;C3] M};[];[];[]] //make this a list of all the possible mills that can be formed
//player will check if there positions contains this mills[0] or mills[1]..
let printBoard b = //print a board b
    let scaler=4; //how big you want the board to be
    let indentScaler=3; // how indented you want board to be
    let numbers=["1";"2";"3";" 4";" 5";"6";"7"];
    let s= (String.concat (sprintf "%*s" (scaler - 1) "") numbers);
    printfn "%*s%s" ((3*indentScaler - 3)*scaler+1) "" s
    let getChar (x:Option<char>) =
        match x with 
        | None -> 'O'
        | Some a -> a
    let printRow (a:Option<char>) (b:Option<char>) (c:Option<char>) layer rowName =
            let initialSpace= ((3-layer)*scaler + 1)
            let spacing =
                match rowName with //spacing depends on row and layer
                  |'D' -> (scaler-1) //minus 1 cause of initial 'O' in first row of D
                  | _ -> (layer*scaler)
            printf "%*c %*c %*c " initialSpace (getChar a) spacing (getChar b) spacing (getChar c)
    let rec print (list:Coords list) = 
        match list with
        | [] -> ()
        | a::b::c::rest ->
            match a.Pos with
            | (row,col) -> 
                match (row,col) with 
                | ('D',1) -> printf "%*c " ((3*indentScaler - 3)*scaler) row
                             printRow a.Status b.Status c.Status a.Layer row//special case
                            // printf "%*s " (scaler-2) "" //print the extra spacing
                | ('D',5) -> printRow a.Status b.Status c.Status a.Layer row
                             printfn ""
                | _->   printf "%*c " ((3*indentScaler - 3)*scaler)  row //print the row
                        printRow a.Status b.Status c.Status a.Layer row
                        printfn ""
            print rest 
         | _ -> ()  
    print b
let getAvailableBoard (playerPositions:(Coords list)) = //returns are board with only available positions
//based on  what the where the players have played
    let rec filterOut (xs:Coords list) out = 
        match xs with
        | []->out
        | a::rest-> filterOut rest (List.filter (fun x -> x.Pos<>a.Pos ) out )
    filterOut playerPositions startBoard  //filterOut


let getCurrentBoard (playerPositions:(Coords list))  =
    let rec changeBoard (xs:Coords list) (out:Coords list) = 
        match xs with
        | []->out
        | a::rest-> changeBoard rest (List.map (fun x -> 
            match x.Pos=a.Pos with
            | true -> {x with Status = a.Status}
            | _ -> x) out ) 
             
    changeBoard playerPositions startBoard  //filterOut


let getCoords (pos:char*int) = //get the Coord type given pos and character to fill it with
    (List.filter (fun x-> x.Pos=pos)startBoard).[0]
     
let getPlayerMove (player:Player) : (char*int) =
    match player.PlayerState with
    | PLACING -> 
        printfn "%s's turn(%d pieces left)" player.Name player.NumberOfPieces
        printf "Row:" 
        let row= Console.ReadLine().[0];
        printf "Column: " 
        let col = int (Console.ReadLine())
        (row,col)
    | MOVING -> ('A',0) //placeholder
    | FLYING -> ('A',0) //placeholder
   // | _ -> failwith "Game is bugged!"
   
let makeMove (coord:char*int) (player:Player) availableBoard = //try make a move using the avaible board
    let length=(List.filter (fun x -> x.Pos=coord) availableBoard).Length;
    //if the length is not 0 this means a position that you tried to move to was taken
    length<>0
    

  //getAvailableBoard 
  //get the avaibleBoard given both player lists of positions
  //check playerState of whichPlayer
  //get input from whichPlayer depending on state
  //check if input from user was valid
  //if move was valid move piece to desired location
  //change player to contain new list of positions placed
  //change numberofPieces and change state if neccessary
let rec runGame (players:Player list) =
    let currentBoard = getCurrentBoard (players.[0].Positions @ players.[1].Positions) //get the state of the board
    printBoard currentBoard //print the board
    let availableBoard = getAvailableBoard (players.[0].Positions @ players.[1].Positions); //the avaialble positions
    let pos= getPlayerMove players.[0]//the positon the player wants to move to
    let updatedPlayer=
        match makeMove pos players.[0] availableBoard with
        | true -> 
                   addPiece players.[0] ({(getCoords pos) with Status = Some (players.[0].Symbol) }) 
                   |> decrementPieces  //move was valid
                   //use piping when using more functions on the player
                  
        | _ ->
            printfn "Pos is %A is not a valid position" pos
            runGame players //make the move again (will make more efficient later)
    runGame [players.[1];updatedPlayer]



let startGame () = 
    runGame Players//start the game

     
          

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    startGame ()
    0 // return an integer exit code
