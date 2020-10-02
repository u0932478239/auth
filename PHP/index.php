<?php
function decrypt_data($encrypted) {
$password = 'enc key'; //Change this to the same key as your program!
$method = 'aes-256-cbc';
$password = substr(hash('sha256', $password, true), 0, 32);
$iv = chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0);
$decrypted = openssl_decrypt(base64_decode($encrypted), $method, $password, OPENSSL_RAW_DATA, $iv);

return $decrypted;
    }
    function encrypt_data($text) {
$password = '3sc3RLrpd17';
$method = 'aes-256-cbc';
$password = substr(hash('sha256', $password, true), 0, 32);
$iv = chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0);
$encrypted = base64_encode(openssl_encrypt($text, $method, $password, OPENSSL_RAW_DATA, $iv));

return $encrypted;
    }
?>
<?php
$u = $_GET['u'];
$user = decrypt_data($u);
$p = $_GET['p'];
$pass = decrypt_data($p);
$expire = $_GET['e'];
$variable = file_get_contents("http://worldclockapi.com/api/json/est/now");
$decoded = json_decode($variable,true);
$time = $decoded["currentDateTime"];
$correctexpire = base64_encode($time);
if ($expire == $correctexpire){
    if ($user == 'Samzy'){
        if ($pass == 'harvest1'){
            echo encrypt_data('access granted');
            die();
        }else{
            echo encrypt_data('password does not match');
            die();
        }
    }else{
        echo encrypt_data('user does not exist');
        die();
    }
}else{
    echo encrypt_data('expired');
    die();
}

?>