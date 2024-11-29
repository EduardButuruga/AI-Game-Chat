from flask import Flask, request, jsonify
import openai
import random

app = Flask(__name__)

# Înlocuiește cu cheia ta API OpenAI
openai.api_key = "Your API OpenAI Key"

# Stocăm istoricul conversațiilor într-un dicționar pentru fiecare sesiune
conversations = {}

@app.route('/chat', methods=['POST'])
def chat():
    data = request.json
    session_id = data.get('session_id', 'default')
    app.logger.debug(f"Received data: {data}")
    message = data.get('message', '')

    if not message:
        return jsonify({'error': 'Message is required'}), 400

    # Adaugă contexte multiple relevante despre universul jocului
    contexts = [
       "Your name is Florentino Perez"
       "You are the manager of Real Madrid"
       "You want to transfer Erling Haaland at Madrid for 100 million euro"
    ]
    
    selected_context = random.choice(contexts)

    if session_id not in conversations:
        conversations[session_id] = [{"role": "system", "content": selected_context}]
    
    # Adăugăm mesajul utilizatorului la istoric
    conversations[session_id].append({"role": "user", "content": message})

    response = openai.ChatCompletion.create(
        model="gpt-3.5-turbo",
        messages=conversations[session_id],
        temperature=0.7,
        max_tokens=150,
        top_p=0.9,
        frequency_penalty=0.5
    )

    response_message = response.choices[0].message['content'].strip()
    app.logger.debug(f"OpenAI response: {response_message}")

    # Adăugăm răspunsul AI-ului la istoric
    conversations[session_id].append({"role": "assistant", "content": response_message})

    return jsonify({'response': response_message})

if __name__ == '__main__':
    app.run(debug=True)