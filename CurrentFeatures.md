### Account Setup:

1. The user creates an account and sets a **login password** (used for authentication).
2. The client generates an **RSA key pair** (public and private keys).
3. The user is given a **12-word passphrase** (mnemonic) for encrypting their private RSA key.
4. The private RSA key is encrypted **client-side** using a key derived from the 12-word passphrase (via **Argon2** or **PBKDF2**).
5. The **encrypted private key** and **public key** are sent to the server for storage, but the **12-word passphrase** remains on the client.

### File Encryption and Upload:

1. The client encrypts files using the **Twofish algorithm**.
2. The **Twofish key** is encrypted using the user's **public RSA key**.
3. The **encrypted file** and **encrypted Twofish key** are uploaded to the server.

### Accessing the Account from a New Device:

1. The user logs in using their **login password** (standard authentication).
2. The server sends the **encrypted private RSA key** to the new client.
3. The user enters their **12-word passphrase** to decrypt the private RSA key locally.
4. The client can now decrypt the **Twofish key** and access files.

### Private Key Recovery (Device Loss):

1. If the user loses their device, they log into their account on a new device using their **login password**.
2. The **encrypted private RSA key** is retrieved from the server.
3. The user enters their **12-word passphrase** to decrypt the private RSA key locally.
