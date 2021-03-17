const jsSHA = require("jsSHA");
const { argv } = require("yargs");

const { secretKey, message } = argv;

var shaObj = new jsSHA("SHA-256", "TEXT");
shaObj.setHMACKey(secretKey, "TEXT");
shaObj.update(message);
var loginChallenge = shaObj.getHMAC("HEX");

console.log(loginChallenge);
