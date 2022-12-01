using Partiality.Modloader;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using Music;

[module: UnverifiableCode]
[assembly: AssemblyCopyright("Copyright ©   2021")]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace DroughtPatch
{
	public class PatchMod : PartialityMod
	{
		public PatchMod()
		{
			instance = this;
			this.ModID = "Drought Patch";
			this.Version = "0100";
			this.author = "HelloThere";
		}

		public static PatchMod instance;

		public delegate void ConvoPatch(LMOracleBehaviorHasMark.MoonConversation self);
		public delegate void FPConvoPatch(FPOracleBehaviorHasMark.PebblesConversation self);
		public delegate void SRSPatch(MessageScreen.SRSConversation self);
		public delegate void UpdatePatch(PearlConversation self, bool eu);
		public delegate void SwallowPatch(PearlConversation self, PhysicalObject item);
		public delegate void MoonPearlPatch(PearlConversation.MoonConversation self);

		public static bool finalMessage = false;
		public static bool playedMusic = false;

		public override void OnEnable()
		{
			base.OnEnable();

			On.GhostConversation.AddEvents += new On.GhostConversation.hook_AddEvents(GhostPatch);
			On.RainWorldGame.RawUpdate += new On.RainWorldGame.hook_RawUpdate(RawUpdatePatch);
			new Hook(
				typeof(LMOracleBehaviorHasMark.MoonConversation).GetMethod("AddEvents", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(PatchMod).GetMethod("MinConvoPatch"));
			new Hook(
				typeof(LMOracleBehaviorHasMark.MoonConversation).GetMethod("PearlIntro", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(PatchMod).GetMethod("PearlPatch"));
			new Hook(
				typeof(FPOracleBehaviorHasMark.PebblesConversation).GetMethod("AddEvents", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(PatchMod).GetMethod("FPEventPatch"));
			new Hook(
				typeof(MessageScreen.SRSConversation).GetMethod("AddEvents", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(PatchMod).GetMethod("SRSEventPatch"));
			new Hook(
				typeof(PearlConversation).GetMethod("Update", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(PatchMod).GetMethod("PearlUpdatePatch"));
			new Hook(
				typeof(PearlConversation).GetMethod("PlayerSwallowItem", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(PatchMod).GetMethod("PearlSwallowPatch"));
			new Hook(
				typeof(PearlConversation.MoonConversation).GetMethod("AddEvents", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance),
				typeof(PatchMod).GetMethod("MoonEventPatch"));
		}

		public static void MinConvoPatch(ConvoPatch orig, LMOracleBehaviorHasMark.MoonConversation self)
		{
			switch (self.id)
			{
				case Conversation.ID.MoonFirstPostMarkConversation:
					self.events.Add(new Conversation.TextEvent(self, 2, self.Translate("Oh..."), 2));
					self.events.Add(new Conversation.TextEvent(self, 4, self.Translate("Hello, little creature."), 8));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("What exactly are you? My systems are too deteriorated to remember."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Well, I appreciate the company. It's been so long<LINE>since I've talked to someone..."), 15));
					break;
				case (Conversation.ID)39:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It seems to be a pearl related to our local intake system."), 13));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ah, I do remember this!"), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("We didn't think the rot could spread as rapidly as it did."), 13));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("We suddenly found out that the microbes inhabiting and<LINE>controlling the intake system had mutated into corruption spores."), 18));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("And then, out of nowhere, the gravitational core was seized by rot tumors,<LINE>and then they quickly spread to rest of the facility."), 20));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("From there, they moved onto both mine and Five Pebbles' grounding pillars,<LINE>and they continued climbing until they had completely consumed our systems."), 22));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It's how I ended up in the state I am now."), 10));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Last I heard, Five Pebbles was doing much better than me, but it's been<LINE>so long since I've been able to contact him."), 16));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I hope he's alright..."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("He is my brother after all."), 8));
					break;
				case (Conversation.ID)40:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...I'm sorry little one, but I can't seem to be able to read this."), 8));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It's written in a complex internal language which my processors can barely<LINE>understand anymore."), 12));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("The only thing I can get out is that it mentions a project to<LINE>harvest the karmic properties of local fauna."), 14));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("But any planned advanced projects have been abandoned by us iterators long ago."), 10));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Our only priority now is try to slow down the advance of the rot as long as possible,<LINE>and to try and gain an understanding on what exactly this all means for us."), 18));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ultimately, we all know what's going to happen."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("The rot will slowly consume us one by one, until nothing remains."), 8));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("In a way, I'm already out of the equation."), 4));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("My processing powers on auxiliary support, my neuron swarms reduced to less than a dozen,<LINE>my emergency generators destroyed..."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It's clear I only have a few cycles left."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("My brother, perhaps some more."), 3));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("The rest, I don't know."), 3));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("But, oh well! I wouldn't want to bother you with the grand schemes of fate, little creature!"), 10));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("That's enough negativity for several cycles."), 5));
					break;
				case (Conversation.ID)41:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Hmm, I don't remember this..."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Apparently it's a log I recorded of a sky-sail journey long, long ago."), 8));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("The tone is very unlike myself."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It's very poetic and nostalgic, reminiscing of old times and wondering about the future..."), 12));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...I guess I got a little carried away by my emotions at the time."), 8));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("From what I can gather I do remember witnessing sky-sail journeys as being quite...cathartic."), 13));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Thank you, <PlayerName>, I suppose."), 8));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I do believe my old self would've been quite grateful to have this back."), 10));
					break;
				case Conversation.ID.Moon_Pearl_CC:
					self.State.InfluenceLike(1f);
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ah, a pearl? And it has such a beautiful color too! I will try my best to read it out to you."), 26));  //13
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 10)); //5
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Hmm, interesting... The header indicates it contains a log of a Type 5 Emergency Message.<LINE>...Authored by one Seven Red Suns."), 36)); //18
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...The name sounds familiar, but I can't seem to remember it..."), 20)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...May have been one of my colleagues in the local group..."), 18)); //9
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Type 5 Emergency Message...?"), 10)); //5
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...That can only mean one thing..."), 14)); //7
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 10)); //5
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Exhaustive incapacitation."), 10)); //5
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 10)); //5
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Ah."), 14)); //7
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("The log contains a fully realized space-time simulation."), 18)); //9
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Going back thousands...no, millions of cycles?!"), 16)); //8
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...I'm sorry little creature, but I can't read the rest of this message to you."), 26)); //13
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("The processors within my systems meant to decode<LINE>such advanced simulations have been damaged beyond repair many many cycles ago."), 38)); //19
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I'm not even sure how this Seven Red Suns was capable of even acomplishing<LINE>such a feat."), 30)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Must have taken thousands of cycles, at the very least."), 20)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Ack!"), 20)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Oh no..."), 20)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It seems the message within the pearl has been corrupted..."), 24)); //12
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Such an advanced and complex simulation...<LINE>It was too delicate to hold itself together for long."), 34)); //17
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I'm surprised it could last unharmed within you throughout your journey..."), 26)); //13
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Why did you come to me?<LINE>Five Pebbles could've probably done a better job at reading that message than me...<LINE>If he's even still alive at this point, that is..."), 50)); //25
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Well, it's too late to regret that now..."), 14)); //7
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I'm really sorry, little creature, you've made such a long and perilous journey in vain..."), 30)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("If it consoles you any bit, feel free to stay within my systems bus for as long as you like.<LINE>It must be one of the very few remaining rot-free places in this world at this point."), 54)); //27
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Or, if you're brave, you could try venturing into the depths and ascending..."), 30)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I'm afraid the choice is only yours."), 12)); //6
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It was nice to see you, little creature.<LINE>It's the first time I've been able to have a nice chat with someone in such a long time."), 44)); //22
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Until we meet again."), 30)); //15
					break;
				case Conversation.ID.MoonPostMark:
					break;
				case (Conversation.ID)10:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("This pearl is just plain text. I will read it to you."), 13)); //13
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("'On regards of the (by spiritual splendor eternally graced) people of the Congregation of Never Dwindling Righteousness,<LINE>we Wish to congratulate (o so thankfully) this -'"), 25)); //25
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Ah."), 7)); //7
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Thank you for this pearl, little one, it brings back nostalgic memories, but I'm afraid I can't read it to you,<LINE>I can't spare the processing power needed to read it whole."), 25)); //25
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("As much as I disagree with my brother on this issue, I must admit our creators certainly weren't..."), 17)); //17
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Well, let's say they weren't quite sparing on pearl data."), 13)); //13
					break;
				case (Conversation.ID)11:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ah, this is an old prayer plate..."), 6)); //6
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I'm sorry little one, but I can't read it to you..."), 12)); //12
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Its encoding method is so old and incredibly obsolete, I've pushed away its decoding systems<LINE>long ago to leave space for more vital processes."), 24)); //24
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Though the ritual scrapings in this plate's shell seem to indicate<LINE>it is a typical old religious poem on the five urges..."), 21)); //21
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I don't know why you'd want to bring this to me, little creature."), 13)); //13
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Such methods and such rites have long since been disproven,<LINE>the people who practised them having abandoned us long ago."), 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Neither you nor me have much time remaining in this world, little one."), 14)); //14
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("You'd make a much better use of it attempting ascension..."), 13)); //13
					break;
				default:
					orig.Invoke(self);
					break;
			}
		}

		public static void PearlPatch(ConvoPatch orig, LMOracleBehaviorHasMark.MoonConversation self)
        {
			if (self.State.totalItemsBrought + self.State.miscPearlCounter == 0)
            {
				self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ah, you would like me to read this?"), 10));
				self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("My processors aren't what they used to<LINE>be, but I'll try my best. Hold on..."), 15));
			}
			else { orig.Invoke(self); }
        }

		public static void FPEventPatch(FPConvoPatch orig, FPOracleBehaviorHasMark.PebblesConversation self)
        {
			switch (self.id)
            {
				case Conversation.ID.MoonFirstPostMarkConversation:
					self.events.Add(new Conversation.TextEvent(self, 4, self.Translate("Ah..."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Hello, little purposed organism."), 8));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I see you're one of the old messenger creatures,<LINE>I recognize your tail markings."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("But why would anyone send one of your kind to me?<LINE>Manual delivery has long been deemed impossible due to<LINE>external conditions."), 23));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Although you're probably not the first of your batch..."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Hm, perhaps I'm just overthinking it, and you simply seek refuge from the rot."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Well, as long as you don't prove to be a nuisance,<LINE>feel free to stay as long as you like."), 18));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Some company is appreciated."), 10));
					break;
				case Conversation.ID.MoonSecondPostMarkConversation:
					break;
				case Conversation.ID.Moon_Misc_Item:
					if (self.describeItem == FPOracleBehaviorHasMark.MiscItemType.LMOracleSwarmer)
                    {
						self.AddBetrayal();
					}
					else { orig.Invoke(self); }
					break;
				case Conversation.ID.Moon_Pearl_Drought1:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ah, this is an old report on the local water intake systems,<LINE>back before it was overrun by the rot."), 20));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("You see, normally each iterator is constructed alongside an individual<LINE>water intake system which filters the water entering it and<LINE>stores it in a reservoir so that the iterator has a constant clean water supply."), 30));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("However, since I was built so close to Looks to the Moon, our creators decided<LINE>that it would be a better idea to expand her intake system and<LINE>have us share it."), 25));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("So when the rot began to fester on it, it had a quick access to both of us iterators<LINE>thanks to the system's connection to our legs."), 23));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("However, for reasons still unknown to me, the rot expanded much more agressively onto Moon<LINE>than it did onto me, almost like it was targeting her."), 24));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("In merely a hundred cycles, it had completely overwhelmed her systems and caused a partial collapse."), 24));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It was a miracle she didn't reach a full collapse, like NSH did."), 18));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Even now, I still wonder how the rot knew how to attack her so systematically."), 20));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Why not me?"), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I've tried my best to move away from those kinds of thoughts."), 18));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Anyway."), 8));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("The information in this pearl has lost signifance long ago."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I don't know why you would want to bring it to me."), 13));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Eh, it's probably just another shiny rock to you."), 13));
					break;
				case Conversation.ID.Moon_Pearl_Drought2:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ah, I recognize this."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It's a data dump relating to an old project by NSH to archive the karmic properties<LINE>and relations of local fauna."), 23));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("This archive would then be used by him to create a series of purposed organisms which<LINE>would be in perfect balance with their surroundings and would therefore<LINE>achieve atonement, something that was previously restricted to intelligent creatures."), 35));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("His idea was to then inject his own genetic code into these organisms,<LINE>so when they ascended, he would ascend too."), 23));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("But this project was a total failure."), 7));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Not only was this organism's karmic alignment completely off-balance,<LINE>they became prime early hosts for the rot."), 23));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Most interestingly though, some of them started spreading into the wilderness,<LINE>passing their genes into the local fauna."), 30));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("This resulted in the rise of a new set of wild organisms that had subconsciously<LINE>inherited a great curiosity in iterators, and were naturally drawn to them."), 28));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("For countless cycles I had to deal with these creatures, constantly entering my chamber<LINE>and interrupting my tasks."), 24));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("My sister had to deal with a similar problem, but she seemed rather happy about it."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Most notably, this species eventually managed to refine their karma and achieve perfect alignment,<LINE>which had been NSH's original goal."), 26));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("We codenamed them the Monks."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("They looked very similar to you, but their skin instead shined a bright yellow hue."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("But by this time the rot had already began to spread, and NSH had already collapsed."), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("There's probably still a few packs of Monks running around, alongside some of their less aligned brothers."), 20));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Maybe you've seen some of them?"), 7));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...I'll take that as a no."), 7));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("After all, the very fact that you exist seems inexplicable to me."), 15));
					break;
				case Conversation.ID.Moon_Pearl_Drought3:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("This is a qualia written by Moon..."), 10));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Little purposed organism, where did you find this...?"), 15));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 5));
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It's very emotional..."), 7));
					self.events.Add(new Conversation.TextEvent(self, 5, self.Translate(". . ."), 10));
					self.events.Add(new Conversation.TextEvent(self, 5, self.Translate("...I-I'm sorry little one, I can't bring myself to read it..."), 15));
					break;
				case Conversation.ID.MoonPostMark:
					break;
				case Conversation.ID.Moon_Pearl_CC:
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("A pearl? What would anyone want to deliver at such a critical time?"), 34)); //17
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...I see."), 20)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("A Type 5 Emergency Signal, from Seven Red Suns."), 26)); //13
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("If he was willing to send a purposed organism on such a dangerous journey<LINE>just to bring this to me, then it must be very important."), 50)); //25
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 10)); //5
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Impressive."), 30)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("A fully realized space-time simulation."), 24)); //12
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Going back to..."), 30)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("..."), 10)); //5
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Ah."), 20)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Well that certainly explains many things."), 36)); //18
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("You see, little one, over the countless cycles that I have existed,<LINE>I've come to regret many things."), 40)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Things that I could've done.<LINE>Things that I could've avoided doing."), 32)); //16
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("But this.<LINE>This brings me a sense of...<LINE>Closure, I could say."), 32)); //16
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ultimately, Moon couldn't have been saved."), 24)); //12
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Ultimately, we shall all perish anyway,<LINE>and this world will be born anew."), 36)); //18
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It's ironic."), 14)); //7
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("It also makes me sickly curious..."), 24)); //12
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("...Anyway."), 30)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("What this means to you is that you are the last one."), 30)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("The rest of your kind have already escaped this nightmarish world."), 32)); //16
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("My last will is to help at least someone flee this madness."), 30)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("Go west, past the Farm Arrays, right below the entrace to NSH's lair,<LINE>and deep underground where it all began."), 44)); //22
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("If you're lucky, you will escape unharmed."), 24)); //12
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I will be watching over you."), 20)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("We will see each other again soon."), 40)); //20
					self.FPOracleBehaviorHasMark.rainWorld.progression.currentSaveState.miscWorldSaveData.pebblesSeenGreenNeuron = true;
					break;
				default:
					orig.Invoke(self);
					break;
            }
        }

		public static void SRSEventPatch(SRSPatch orig, MessageScreen.SRSConversation self)
        {
			self.events.Add(new Conversation.TextEvent(self, 0, "...", 10));
		}

		public static void PearlUpdatePatch(UpdatePatch orig, PearlConversation self, bool eu)
        {
			if (!finalMessage && self.rainWorld.progression.currentSaveState.miscWorldSaveData.pebblesSeenGreenNeuron && self.player.room.abstractRoom.name.Equals("SB_D02"))
			{
				Debug.Log("Moon SI conversation");
				//Debug.Log(self.rainWorld.progression.currentSaveState.miscWorldSaveData.pebblesSeenGreenNeuron);
				//self.rainWorld.progression.currentSaveState.miscWorldSaveData.pebblesSeenGreenNeuron = true;
				self.currentConversation = new PearlConversation.MoonConversation(Conversation.ID.MoonRecieveSwarmer, self);
				self.talking = true;
				self.currentConversation.Update();
				finalMessage = true;
			}
			else { orig.Invoke(self, eu); }
        }

		public static void PearlSwallowPatch(SwallowPatch orig, PearlConversation self, PhysicalObject item)
        {
			if (item is DataPearl)
            {
				Debug.LogError("Override Remote Pearl Read Code, Pearl ID is " + (int)(item as DataPearl).AbstractPearl.dataPearlType);
            }
        }

		public static void MoonEventPatch(MoonPearlPatch orig, PearlConversation.MoonConversation self)
        {
			Debug.Log(self.id.ToString() + " " + self.State.neuronsLeft);
			if (self.id == Conversation.ID.MoonRecieveSwarmer)
            {
				Debug.Log("Works");
				//TODO: Pebbles Dialogue if possible
				self.events.Add(new Conversation.TextEvent(self, 50, self.Translate("I see you're about to reach the end of your journey, little creature."), 80));
				self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("I hope beyond the layers of stone you can finally reunite with your loved ones once again.<LINE>Or maybe you'll find something else..."), 140));
				self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("You must be one of the few remaining lucky creatures to escape this world safely.<LINE>Soon us iterators will follow..."), 120));
				self.events.Add(new Conversation.TextEvent(self, 0, self.Translate("We will see each other again soon..."), 60));
				Debug.Log("List measures: "+ self.events.Count);
			}
        }

		public static void GhostPatch(On.GhostConversation.orig_AddEvents orig, GhostConversation self)
        {
			switch (self.id)
            {
				case Conversation.ID.Ghost_CC:
					self.events.Add(new Conversation.TextEvent(self, 0, "Look down...", 10)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, "The world has turned into a living nightmare.", 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, "Once it was full of life.", 15)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, "I would climb upon this spot to observe the rising of the sun...", 25)); //25
					self.events.Add(new Conversation.TextEvent(self, 0, "We destroyed it.", 10)); //10
					self.events.Add(new Conversation.TextEvent(self, 0, "If only we hadn't been so selfish...", 30)); //30
					break;
				case Conversation.ID.Ghost_SI:
					self.events.Add(new Conversation.TextEvent(self, 0, "The voices are gone.", 15)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, "The buzzing in the air? Gone.", 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, "The whispering of the wind? Gone.", 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, "It is the silence of death.", 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, "Peace has finally come.", 30)); //30
					break;
				case Conversation.ID.Ghost_LF:
					self.events.Add(new Conversation.TextEvent(self, 0, "Everything has changed so much...", 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, "What happened to my Arrays?", 15)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, "They are full of monsters.", 15)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, "What are these creatures, consuming and destroying everything in their path?", 40)); //40
					self.events.Add(new Conversation.TextEvent(self, 0, "They are the consequences of our actions.", 25)); //25
					self.events.Add(new Conversation.TextEvent(self, 0, "And now I am condemned to watch them destroy everything I ever knew.", 60)); //60
					break;
				case Conversation.ID.Ghost_SH:
					self.events.Add(new Conversation.TextEvent(self, 0, "Look at these temples...", 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, "We were so foolish back then.", 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, "Believing we could do anything with no consequences...", 35)); //35
					self.events.Add(new Conversation.TextEvent(self, 0, "We only doomed ourselves.", 40)); //40
					break;
				case Conversation.ID.Ghost_UW:
					self.events.Add(new Conversation.TextEvent(self, 0, "Do you know what you're stepping in?", 25)); //25
					self.events.Add(new Conversation.TextEvent(self, 0, "This was our gift to this world. A show of our power.", 35)); //35
					self.events.Add(new Conversation.TextEvent(self, 0, "A gigantic machine of infinite power, made to serve us.", 35)); //35
					self.events.Add(new Conversation.TextEvent(self, 0, "But we made a terrible mistake.<LINE>We shouldn't have played Gods.", 40)); //40
					self.events.Add(new Conversation.TextEvent(self, 0, "And just like us, desperation consumed them.<LINE>And just like us, they will soon fall to their own twisted creation.", 100)); //100
					break;
				case Conversation.ID.Ghost_SB:
					self.events.Add(new Conversation.TextEvent(self, 0, "Ah, how amusing...", 15)); //15
					self.events.Add(new Conversation.TextEvent(self, 0, "Even now, the irony of your existence still baffles me...", 35)); //35
					self.events.Add(new Conversation.TextEvent(self, 0, "You and I are quite related, you know...", 25)); //25
					self.events.Add(new Conversation.TextEvent(self, 0, "Not just by common experience.", 20)); //20
					self.events.Add(new Conversation.TextEvent(self, 0, "But now it is time for this world to end,<LINE>and for the cycle to begin anew once again...", 60)); //60
					self.events.Add(new Conversation.TextEvent(self, 0, "See you at the other side...", 40)); //40
					break;
			}
        }

		public static void RawUpdatePatch(On.RainWorldGame.orig_RawUpdate orig, RainWorldGame self, float dt)
        {
			orig.Invoke(self, dt);
			MusicPlayer musicPlayer = self.rainWorld.processManager.musicPlayer;
			if (!playedMusic && musicPlayer != null && musicPlayer.song == null && self.Players[0].Room.name == "LM_E01")
            {
				musicPlayer.RequestArenaSong("NA_28 - Stargazer", 200f);
				playedMusic = true;
			}
        }
	}
}
