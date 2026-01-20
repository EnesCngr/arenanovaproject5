// Firebase Helper Functions for Blazor Interop
import { 
    signInWithEmailAndPassword, 
    createUserWithEmailAndPassword, 
    signOut,
    onAuthStateChanged 
} from 'https://www.gstatic.com/firebasejs/12.7.0/firebase-auth.js';

import { 
    collection, 
    doc, 
    getDoc, 
    getDocs, 
    setDoc, 
    addDoc, 
    updateDoc, 
    deleteDoc,
    query,
    where
} from 'https://www.gstatic.com/firebasejs/12.7.0/firebase-firestore.js';

// ============= Authentication Functions =============

window.firebaseSignIn = async function(email, password) {
    try {
        const userCredential = await signInWithEmailAndPassword(window.firebaseAuth, email, password);
        return {
            success: true,
            userId: userCredential.user.uid,
            email: userCredential.user.email
        };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firebaseSignUp = async function(email, password) {
    try {
        const userCredential = await createUserWithEmailAndPassword(window.firebaseAuth, email, password);
        return {
            success: true,
            userId: userCredential.user.uid,
            email: userCredential.user.email
        };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firebaseSignOut = async function() {
    try {
        await signOut(window.firebaseAuth);
        return { success: true };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firebaseGetCurrentUser = function() {
    const user = window.firebaseAuth.currentUser;
    if (user) {
        return {
            userId: user.uid,
            email: user.email,
            displayName: user.displayName
        };
    }
    return null;
};

// ============= Firestore Functions =============

window.firestoreGetDocument = async function(collectionName, documentId) {
    try {
        const docRef = doc(window.firebaseDB, collectionName, documentId);
        const docSnap = await getDoc(docRef);
        
        if (docSnap.exists()) {
            return {
                success: true,
                data: JSON.stringify({ id: docSnap.id, ...docSnap.data() })
            };
        } else {
            return {
                success: false,
                error: "Document not found"
            };
        }
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firestoreGetCollection = async function(collectionName) {
    try {
        const querySnapshot = await getDocs(collection(window.firebaseDB, collectionName));
        const documents = [];
        
        querySnapshot.forEach((doc) => {
            documents.push({ id: doc.id, ...doc.data() });
        });
        
        return {
            success: true,
            data: JSON.stringify(documents)
        };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firestoreSetDocument = async function(collectionName, documentId, dataJson) {
    try {
        const data = JSON.parse(dataJson);
        await setDoc(doc(window.firebaseDB, collectionName, documentId), data);
        return { success: true };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firestoreAddDocument = async function(collectionName, dataJson) {
    try {
        const data = JSON.parse(dataJson);
        const docRef = await addDoc(collection(window.firebaseDB, collectionName), data);
        return {
            success: true,
            documentId: docRef.id
        };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firestoreUpdateDocument = async function(collectionName, documentId, dataJson) {
    try {
        const data = JSON.parse(dataJson);
        const docRef = doc(window.firebaseDB, collectionName, documentId);
        await updateDoc(docRef, data);
        return { success: true };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firestoreDeleteDocument = async function(collectionName, documentId) {
    try {
        await deleteDoc(doc(window.firebaseDB, collectionName, documentId));
        return { success: true };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

window.firestoreQueryCollection = async function(collectionName, field, operator, value) {
    try {
        const q = query(collection(window.firebaseDB, collectionName), where(field, operator, value));
        const querySnapshot = await getDocs(q);
        const documents = [];
        
        querySnapshot.forEach((doc) => {
            documents.push({ id: doc.id, ...doc.data() });
        });
        
        return {
            success: true,
            data: JSON.stringify(documents)
        };
    } catch (error) {
        return {
            success: false,
            error: error.message
        };
    }
};

console.log('Firebase helper functions loaded successfully');
