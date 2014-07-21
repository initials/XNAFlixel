<?php
 
require_once "connect.php";



// functions:
switch($_GET['f']) {
case 'addData':
    addData();
	break;
case 'getWorldWideTotal':
    getWorldWideTotal();
	break;	
case 'getCompleteStats':
	getCompleteStats();
	break;
case 'getCompleteStatsAsHTMLTable':
	getCompleteStatsAsHTMLTable();
	break;

	
case 'getHighScore':
    getHighScore();
	break;	
case 'getTotalForLevel':
	getTotalForLevel();
	break;
case 'createTableForGame':
    createTableForGame();
	break;	
}



function createTableForGame() {
/*
usage = 

http://initialsgames.com/highscores/commands.php?f=createTableForGame&gamename=sharehouse

*/

	$gamename = $_GET['gamename'];
	$sqlx = "CREATE TABLE IF NOT EXISTS ".$gamename."( 
	levelname varchar(55),
	highscore INT,
	totalscore INT,
	user varchar(255) ,
	actor varchar(255),
	PRIMARY KEY(user))";
	$queryx = mysql_query($sqlx);
	
}


/*
usage = 
http://initialsgames.com/highscores/commands.php?f=getHighScore&gamename=fourchambers
*/
function getHighScore() {
	$gamename = $_GET['gamename'];
	$getscore = "SELECT * FROM ".$gamename." ORDER BY `highscore`  DESC LIMIT 0, 1 ";
	$query = mysql_query($getscore);
	
	while($row = mysql_fetch_array($query))
	{
		echo "User: ".$row['user']." High Score: ".$row['highscore']." Total Score: ".$row['totalscore'];
	}
}

function getWorldWideTotal() {

/*
SELECT    SUM(value)
FROM      myTable

usage = 

http://initialsgames.com/highscores/commands.php?f=getWorldWideTotal&gamename=lemonadecratebox


*/
	$gamename = $_GET['gamename'];
	$getscore = "SELECT SUM(`totalscore`) FROM ".$gamename."";
	$query = mysql_query($getscore);
	
	while($row = mysql_fetch_array($query))
	{
		echo "".$row[0];
	}
	
	
}


function getTotalForLevel() {

/*
SELECT    SUM(value)
FROM      myTable

usage = 

http://initialsgames.com/highscores/commands.php?f=getTotalForLevel&gamename=hawksnest&user=marksman


*/
	$user = $_GET['user'];
	$gamename = $_GET['gamename'];
	$getscore = "SELECT * FROM ".$gamename." WHERE `user` = '".$user."' ";
	
	$query = mysql_query($getscore);
	
	while($row = mysql_fetch_array($query))
	{
		echo "".$row['totalscore'];
	}
	
	
}
function getHighScoresStatsAsHTMLTable() {
	$orderby = $_GET['orderby'];
	$gamename = $_GET['gamename'];
	$getscore = "SELECT * FROM ".$gamename." ORDER BY ". $orderby ." DESC LIMIT 0, 100";
	
	$query = mysql_query($getscore);
	echo "<table border=\"0\">";
	echo "<tr>";
	echo "<td>Name</td><td>High Score</td><td>Total Score</td>";
	echo "</tr>";
	
	while($row = mysql_fetch_array($query))
	{
		echo "<tr>";
		echo "<td>".$row['user']." </td><td> ".$row['highscore']." </td><td> ".$row['totalscore']." </td>";
		echo "</tr>";
	}
	echo "</table>";
}

function getCompleteStatsAsHTMLTable() {
	$orderby = $_GET['orderby'];
	$gamename = $_GET['gamename'];
	$getscore = "SELECT * FROM ".$gamename." ORDER BY ". $orderby ." DESC LIMIT 0, 100";
	
	$query = mysql_query($getscore);
	echo "<table border=\"0\">";
	echo "<tr>";
	echo "<td>Rank</td><td>Name</td><td>High Score</td>";
	echo "</tr>";
	
	$count = 1;
	
	while($row = mysql_fetch_array($query))
	{
		echo "<tr>";
		$sx = substr( $row['user'], 0, 1 ) === "@";
		if ($sx===true) {
			echo "<td>".$count."</td><td><a href=\"http://www.twitter.com/".$row['user']."\">".$row['user']."</a>  </td><td> ".$row[$orderby]." </td>";
		}
		else 
		{
			echo "<td>".$count."</td><td> ".$row['user']." </td><td> ".$row[$orderby]." </td>";
		}
		$count++;
		echo "</tr>";
	}
	echo "</table>";
}

function getCompleteStats() {

/*
SELECT    SUM(value)
FROM      myTable

usage = 

http://initialsgames.com/highscores/commands.php?f=getCompleteStats&gamename=hawksnest&orderby=highscore


*/
	$orderby = $_GET['orderby'];
	$gamename = $_GET['gamename'];
	$getscore = "SELECT * FROM ".$gamename." ORDER BY ". $orderby ." DESC LIMIT 0, 20";
	
	//echo $getscore;
	
	$query = mysql_query($getscore);

	while($row = mysql_fetch_array($query))
	{

		echo $row['user'].",".$row['highscore'].",".$row['totalscore'].",,";

	}
}


/*

usage = 

http://initialsgames.com/highscores/commands.php?f=addData&levelname=warehouse2&score=100&gamename=lemonadecratebox

*/
function addData() {
	$gamename = $_GET['gamename'];
	$level = $_GET['level'];
	$levelname = $_GET['levelname'];
	$score = $_GET['score'];
	$user = $_GET['user'];
	$actor = $_GET['actor'];
	$previoushighscore = 0;
	$previoustotalscore = 0;
		
	
	$getscore = "SELECT * FROM ".$gamename." WHERE `user` = '".$user."' ";
	$query = mysql_query($getscore);
	if ($query) {
		if (mysql_num_rows($query) == 0) {
			
		}
		else {
			while($row = mysql_fetch_array($query))
			{
				$previoushighscore = $row['highscore'];
				$previoustotalscore = $row['totalscore'];
			}
		}
	}
	
	//echo 'last='.$previoushighscore.' -- '.$previoustotalscore;
	
	$newtotal = $previoustotalscore + $score;
	
	if ($score > $previoushighscore) {
		$sql = "INSERT INTO ".$gamename." (levelname, totalscore, highscore, user) VALUES ('$levelname', '$newtotal', '$score', '$user') ON DUPLICATE KEY UPDATE levelname='$levelname', totalscore='$newtotal', user='$user', highscore='$score';";

		$query = mysql_query($sql);

		if ($query === TRUE) {
		 
			//echo 'You have successfully added some info, and have a high score!';
			echo 'HISCORE=YES';
		}
		else 
		{
			echo 'HISCORE=YES, Error, information not added.';
		}
	
	}
	
	else {
		$sql = "INSERT INTO ".$gamename." (user, totalscore) VALUES ('$user', '$newtotal') ON DUPLICATE KEY UPDATE user='$user', totalscore='$newtotal';";

		$query = mysql_query($sql);

		if ($query === TRUE) {
		 
			//echo 'You have successfully added some info';
			echo 'HISCORE=NO';
		}
		else 
		{
			echo 'HISCORE=NO, Error, information not added.';
		}
	
	}


}


?>